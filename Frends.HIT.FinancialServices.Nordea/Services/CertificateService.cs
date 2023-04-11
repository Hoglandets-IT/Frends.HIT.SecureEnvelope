using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace Frends.HIT.FinancialServices.Nordea.Services
{
    public class CertificateService
    {
        private const string CertCheck = "-----BEGIN CERTIFICATE-----";
        public static X509Certificate2 GetX509Certificate(string certificate)
        {
            if (string.IsNullOrEmpty(certificate))
            {
                throw new Exception(message: "Certificate is empty or null");
            }
            var certAndKey = GetCertAndKey(certificate);
            return X509Certificate2.CreateFromPem(certAndKey["certPem"], certAndKey["keyPem"]);
        }

        private static IDictionary<string, string> GetCertAndKey(string cert)
        {
            var list = cert.Split(",");
            IDictionary<string, string> certAndKey = new Dictionary<string, string>();
            foreach (var str in list)
            {
                certAndKey.Add(str.StartsWith(CertCheck) ? "certPem" : "keyPem", str);
            }
            return certAndKey;
        }
    }
}
