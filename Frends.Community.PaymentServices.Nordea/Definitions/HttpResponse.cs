using System.Collections.Generic;

#pragma warning disable 1591

namespace Frends.Community.PaymentServices.Nordea.Definitions
{
    public class HttpResponse
    {
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public int StatusCode { get; set; }
    }
}
