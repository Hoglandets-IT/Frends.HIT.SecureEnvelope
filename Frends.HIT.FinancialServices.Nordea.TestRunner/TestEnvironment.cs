using Frends.HIT.FinancialServices.Nordea;
using Frends.HIT.FinancialServices.Nordea.Definitions;
using Newtonsoft.Json.Linq;

namespace TestRunner;

public static class TestEnvironment
{
    private const string CustomerId = "11111111";
    private const string Environment = "PRODUCTION";
    private const string Url = @"https://filetransfer.nordea.com:443/services/CorporateFileService";
    private const int RequestId = 1;
    private const int ConnectionTimeOutSeconds = 30;
    private const string Status = "ALL";
    private const string FileType = "VKEUR";
    private const string TargetId = "11111111A1";
    private const string StartDate = "2018-05-25";
    private const string EndDate = "2023-05-25";
    private const string FileEncoding = "utf-8";

    public static async Task<JToken> GetUserInfo()
    {
        var userInfo = new GetUserInfoInput
        {
            CustomerId = CustomerId,
            Environment = Environment,
            Url = Url,
            RequestId = RequestId,
            ConnectionTimeOutSeconds = ConnectionTimeOutSeconds,
            Certificate = System.Environment.GetEnvironmentVariable("CERTIFICATE") ?? string.Empty
        };

        Console.WriteLine("Start program GetUserInfo");
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        var resp = await WebServices.GetUserInfo(userInfo, token);
        
        return resp;
    }

    public static async Task<JToken> DownloadFileList()
    {
        var fileListInput = new FileListInput()
        {
            CustomerId = CustomerId,
            Environment = Environment,
            Url = Url,
            RequestId = RequestId,
            ConnectionTimeOutSeconds = ConnectionTimeOutSeconds,
            Status = Status,
            FileType = FileType,
            TargetId = TargetId,
            StartDate = StartDate,
            EndDate = EndDate,
            Certificate = System.Environment.GetEnvironmentVariable("CERTIFICATE") ?? string.Empty
        };
        
        Console.WriteLine("Start program DownloadFileList");
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        var resp = await WebServices.DownloadFileList(fileListInput, token);
        
        return resp;
    }

    public static async Task<JToken> UploadFile()
    {
        var uploadFileInput = new UploadFileInput()
        {
            CustomerId = CustomerId,
            Environment = Environment,
            Url = Url,
            FileType = FileType,
            FileEncoding = FileEncoding,
            FileInput = "Test",
            Certificate = System.Environment.GetEnvironmentVariable("CERTIFICATE") ?? string.Empty
        };
        
        Console.WriteLine("Start program UploadFile");
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        var resp = await WebServices.UploadFile(uploadFileInput, token);
        
        return resp;
        
    }
}