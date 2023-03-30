using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace Frends.Community.FinancialServices.Nordea.Services
{
    public class CertificateService
    {
        public static X509Certificate2 GetX509Certificate(string certificate, string privateKey)
        {
            return X509Certificate2.CreateFromPem(certificate, privateKey);
        }
    }
}
