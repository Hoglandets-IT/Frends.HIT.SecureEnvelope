using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

#pragma warning disable 1591

namespace Frends.Community.PaymentServices.Nordea.Services
{
    public static class CertificateService
    {
        // Fetches the first certificate whose CN contains the given string
        public static X509Certificate2 FindCertificate(string issuedBy)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            var certificate = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(c => c.Issuer.Contains($"CN={issuedBy}"));

            if (certificate == null)
            {
                throw new ArgumentException($"Could not find certificate issued by: '{issuedBy}'", nameof(issuedBy));
            }

            var expireDate = DateTime.Parse(certificate.GetExpirationDateString());

            if (expireDate < DateTime.Now)
            {
                throw new Exception($"Certificate has already expired: '{expireDate}'");
            }

            store.Close();

            return certificate;
        }
    }
}
