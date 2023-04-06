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
        static async Task Main(string[] args)
        {
            GetUserInfoInput userinfor = new GetUserInfoInput();
            userinfor.CustomerId = "0000000000";
            userinfor.Environment = "TEST";
            userinfor.Url = @"https://filetransfer.nordea.com:443/services/CorporateFileService";
            userinfor.RequestId = 123456789;
            
            //Add the cert here cert,private key
            userinfor.Certificate = string.Empty;

            Console.WriteLine("Start program");
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var resp = await WebServices.GetUserInfo(userinfor, token);
            //Console.WriteLine(resp.Result.);
            Console.WriteLine(resp.ToString());
        }
    }
}