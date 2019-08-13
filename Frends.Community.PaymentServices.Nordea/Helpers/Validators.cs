using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Environment = Frends.Community.PaymentServices.Nordea.Helpers.Enums.Environment;
using Status = Frends.Community.PaymentServices.Nordea.Helpers.Enums.Status;

#pragma warning disable 1591

namespace Frends.Community.PaymentServices.Nordea.Helpers
{
    class Validators
    {
        public static void ValidateParameters(string url, X509Certificate2 certificate, string environment, params KeyValuePair<string, string>[] stringParameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Url cannot be empty", nameof(url));
            }
            else
            {
                try
                {
                    var uri = new Uri(url);
                }
                catch (UriFormatException e)
                {
                    throw new ArgumentException("Url is not valid", nameof(url), e);
                }
            }

            if (!certificate.HasPrivateKey)
            {
                throw new ArgumentException("Certificate is missing the private key for signing", nameof(certificate));
            }

            if (!Enum.TryParse(environment, out Environment env))
            {
                throw new ArgumentException($"Environment value is not valid. Valid values are: '{Environment.PRODUCTION}' / '{Environment.TEST}'", nameof(environment));
            }

            foreach (var stringParameter in stringParameters)
            {
                ValidateStringParameter(stringParameter);
            }
        }

        public static void ValidateStringParameter(KeyValuePair<string, string> parameter)
        {
            if (string.IsNullOrEmpty(parameter.Value))
            {
                throw new ArgumentException("Value cannot be empty", parameter.Key);
            }
        }

        public static void ValidateStatusParameter(string status)
        {
            if (!Enum.TryParse(status, out Status stat))
            {
                throw new ArgumentException($"Status value is not valid. Valid values are: '{Status.NEW}' / '{Status.DOWNLOADED}' / '{Status.ALL}'", nameof(status));
            }
        }
    }
}
