using Frends.HIT.SecureEnvelope.Definitions;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace Frends.HIT.SecureEnvelope.Test;

public class HttpTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public HttpTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private const string CustomerId = "11111111";
    private const string Environment = "PRODUCTION";
    private const string Url = @"https://filetransfer.nordea.com:443/services/CorporateFileService";
    private const int RequestId = 1;
    private const int ConnectionTimeOutSeconds = 30;
    private const string Status = "ALL";
    private const string FileType = "VKEUR";
    private const string TargetId = "11111111A1";
    private const string StartDate = "2018-05-25";
    private static readonly string EndDate = DateTime.Now.ToString("yyyy-MM-dd");
    private const string FileEncoding = "utf-8";
    private readonly string _dummyCert = System.Environment.GetEnvironmentVariable("DUMMY_CERT") ?? string.Empty;
    
    [Fact]
    public async Task GetUserInfoWithoutCertTest()
    {
        _testOutputHelper.WriteLine("Test to get user info without a certificate");

        var userInfo = new GetUserInfoInput
        {
            CustomerId = CustomerId,
            Environment = Environment,
            Url = Url,
            RequestId = RequestId,
            ConnectionTimeOutSeconds = ConnectionTimeOutSeconds,
            Certificate = string.Empty
        };
        
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        async Task<JToken> Act () => await WebServices.GetUserInfo(userInfo, token);
        var exception = await Assert.ThrowsAsync<Exception>(Act);
        Assert.Equal("Certificate is empty or null", exception.Message);
    }
    
    [Fact]
    public async Task GetUserInfoWithCertTest()
    {
        _testOutputHelper.WriteLine("Test to get user info with a certificate");
        
        var userInfo = new GetUserInfoInput
        {
            CustomerId = CustomerId,
            Environment = Environment,
            Url = Url,
            RequestId = RequestId,
            ConnectionTimeOutSeconds = ConnectionTimeOutSeconds,
            Certificate = _dummyCert
        };
        
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        async Task<JToken> Act () => await WebServices.GetUserInfo(userInfo, token);
        var exception = await Assert.ThrowsAsync<Exception>(Act);
        Assert.Equal("ResponseCode: 24, ResponseText: Content not found., Content: ", exception.Message);
    }
    
    [Fact]
    public async Task DownloadFileListTest()
    {
        _testOutputHelper.WriteLine("Test to get file list");
        
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
            Certificate = _dummyCert
        };
        
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        var resp = await WebServices.DownloadFileList(fileListInput, token);
        var jToken = JToken.Parse(resp.ToString());
        List<JToken> jTokenList = jToken.ToList();
        var listLenght = jTokenList.Count;
        _testOutputHelper.WriteLine($"FileList contains {listLenght} file(s)");
        Assert.True(listLenght >= 1, $"FileList contains 0 file(s)");
        
    }
}