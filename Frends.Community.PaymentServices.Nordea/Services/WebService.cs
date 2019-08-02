using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Frends.Community.PaymentServices.Nordea.Definitions;

#pragma warning disable 1591

namespace Frends.Community.PaymentServices.Nordea.Services
{
    public static class WebService
    {
        private static ConcurrentDictionary<WebServiceSettings, HttpClient> ClientCache = new ConcurrentDictionary<WebServiceSettings, HttpClient>();

        private static HttpClient GetHttpClientForSettings(WebServiceSettings settings, int connectionTimeoutSeconds)
        {

            return ClientCache.GetOrAdd(settings, (sets) =>
            {
                // Might get called more than once if e.g. many process instances execute at once,
                // but that should not matter much, as only one client will get cached
                var handler = new HttpClientHandler();
                handler.SetHandlerSettingsBasedOnSettings(sets);
                var httpClient = new HttpClient(handler);

                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(connectionTimeoutSeconds));

                return httpClient;
            });

        }

        private static IDictionary<string, string> GetHeaderDictionary(string url, string softwareId)
        {
            var uri = new Uri(url);

            //Ignore case for headers and key comparison
            IEnumerable<Header> headers = new Header[] 
            {
                new Header(){ Name = "User-Agent", Value = softwareId},
                new Header(){ Name = "Content-Type", Value = @"text/xml;charset=""utf-8"""},
                new Header(){ Name = "Host", Value = uri.Host},
                new Header(){ Name = "Accept", Value = "gzip,deflate"}
            };
            return headers.ToDictionary(key => key.Name, value => value.Value, StringComparer.InvariantCultureIgnoreCase);
        }

        // Combine response- and responsecontent header to one dictionary
        private static Dictionary<string, string> GetResponseHeaderDictionary(HttpResponseHeaders responseMessageHeaders, HttpContentHeaders contentHeaders)
        {
            var responseHeaders = responseMessageHeaders.ToDictionary(h => h.Key, h => string.Join(";", h.Value));
            var allHeaders = contentHeaders.ToDictionary(h => h.Key, h => string.Join(";", h.Value));
            responseHeaders.ToList().ForEach(x => allHeaders[x.Key] = x.Value);
            return allHeaders;
        }

        private static async Task<HttpResponseMessage> GetHttpRequestResponseAsync(
            HttpClient httpClient, string method, string url,
            HttpContent content, IDictionary<string, string> headers,
            WebServiceSettings settings, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var request = new HttpRequestMessage(new HttpMethod(method), new Uri(url))
            {
                Content = content,
            };

            // Clear default headers
            content.Headers.Clear();
            
            foreach (var header in headers)
            {
                var requestHeaderAddedSuccessfully = request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                if (!requestHeaderAddedSuccessfully && request.Content != null)
                {
                    //Could not add to request headers try to add to content headers
                    var contentHeaderAddedSuccessfully = content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    if (!contentHeaderAddedSuccessfully)
                    {
                        Trace.TraceWarning($"Could not add header {header.Key}:{header.Value}");
                    }
                }
            }

            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (settings.AllowInvalidResponseContentTypeCharSet && response.Content.Headers?.ContentType != null)
            {
                response.Content.Headers.ContentType.CharSet = null;
            }
            return response;

        }

        // The method that sends the SOAP messages to OP web service interface
        public static async Task<HttpResponse> CallWebService(string url, string soapMessage, string softwareId, int connectionTimeoutSeconds, CancellationToken cancellationToken)
        {
            var settings = new WebServiceSettings();

            var httpClient = GetHttpClientForSettings(settings, connectionTimeoutSeconds);
            var headers = GetHeaderDictionary(url, softwareId);

            using (var content = new StringContent(soapMessage, Encoding.GetEncoding(Encoding.UTF8.WebName)))
            {
                var responseMessage = await GetHttpRequestResponseAsync(
                        httpClient,
                        "POST",
                        url,
                        content,
                        headers,
                        settings,
                        cancellationToken)
                    .ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                var response = new HttpResponse()
                {
                    Body = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false),
                    StatusCode = (int)responseMessage.StatusCode,
                    Headers = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers)
                };

                if (!responseMessage.IsSuccessStatusCode && settings.ThrowExceptionOnErrorResponse)
                {
                    throw new WebException(
                        $"Request to '{url}' failed with status code {(int)responseMessage.StatusCode}. Response body: {response.Body}");
                }

                return response;
            }
        }
    }

    public static class Extensions
    {
        internal static void SetHandlerSettingsBasedOnSettings(this HttpClientHandler handler, WebServiceSettings settings)
        {
            // Uses Windows Integrated Security
            handler.UseDefaultCredentials = true;
            handler.AllowAutoRedirect = settings.FollowRedirects;
            // Should this use a proxy?

            //Allow all endpoint types
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
        }
    }
}