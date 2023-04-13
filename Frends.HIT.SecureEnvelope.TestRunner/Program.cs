namespace Frends.HIT.SecureEnvelope.TestRunner
{
    class Program
    {
        static void  Main(string[] args)
        {
           var response = TestEnvironment.GetUserInfo();
           // var response2 =  TestEnvironment.DownloadFileList();
           // var response3 = TestEnvironment.UploadFile();
            Console.WriteLine("Result here: " + response.Result.ToString());
        }
    }
}