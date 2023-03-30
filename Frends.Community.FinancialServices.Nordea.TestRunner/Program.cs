using System;
using Frends.Community.FinancialServices.Nordea;
using Frends.Community.FinancialServices.Nordea.Definitions;
using System.Threading;
using Newtonsoft.Json.Linq;
using Frends.Community.FinancialServices.Nordea.Services;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            GetUserInfoInput userinfor = new GetUserInfoInput();
            userinfor.CustomerId = "162355330";
            userinfor.Environment = "TEST";
            userinfor.Url = @"https://filetransfer.nordea.com:2020/services/CorporateFileService";
            userinfor.Certificate = null;

            userinfor.PrivateKey = null;

            Console.WriteLine("Start program");
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            JToken resp = WebServices.GetUserInfo(userinfor, token);
            Console.WriteLine(resp.ToString());
        }
    }
}