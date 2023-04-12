using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Environment = Frends.HIT.SecureEnvelope.Helpers.Enums.Environment;
using Status = Frends.HIT.SecureEnvelope.Helpers.Enums.Status;

namespace Frends.HIT.SecureEnvelope.Helpers
{
    public class Validators
    {
        public static void ValidateParameters(string url, string certificate, string environment, params KeyValuePair<string, string>[] stringParameters)
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

            if (String.IsNullOrEmpty(certificate))
            {
                throw new ArgumentException("Certificate is missing the private key for signing", nameof(certificate));
            }

            if (!Enum.TryParse(environment, out Enums.Environment env))
            {
                throw new ArgumentException($"Environment value is not valid. Valid values are: '{Enums.Environment.PRODUCTION}' / '{Enums.Environment.TEST}'", nameof(environment));
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
            if (!Enum.TryParse(status, out Enums.Status stat))
            {
                throw new ArgumentException($"Status value is not valid. Valid values are: '{Enums.Status.NEW}' / '{Enums.Status.DOWNLOADED}' / '{Enums.Status.ALL}'", nameof(status));
            }
        }
    }
}
