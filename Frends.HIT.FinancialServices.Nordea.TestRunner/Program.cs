using System;
using Frends.HIT.FinancialServices.Nordea;
using Frends.HIT.FinancialServices.Nordea.Definitions;
using System.Threading;
using Newtonsoft.Json.Linq;
using Frends.HIT.FinancialServices.Nordea.Services;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace TestRunner
{
    class Program
    {
        static void  Main(string[] args)
        {
           // var response = TestEnvironment.GetUserInfo();
           // var response2 =  TestEnvironment.DownloadFileList();
            var response3 = TestEnvironment.UploadFile();
            Console.WriteLine(response3.Result.ToString());
        }
    }
}