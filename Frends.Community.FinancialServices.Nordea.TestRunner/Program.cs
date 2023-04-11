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
        static void  Main(string[] args)
        {
           // var response = TestEnvironment.GetUserInfo();
           // var response2 =  TestEnvironment.DownloadFileList();
            var response3 = TestEnvironment.UploadFile();
            Console.WriteLine(response3.Result.ToString());
        }
    }
}