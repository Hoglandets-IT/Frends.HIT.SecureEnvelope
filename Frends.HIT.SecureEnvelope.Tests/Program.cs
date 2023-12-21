using Frends.HIT.SecureEnvelope;
using Frends.HIT.SecureEnvelope.Definitions;
using Frends.HIT.SecureEnvelope.Tests;

// Open and read LocalInput.xml
var inputContent = File.ReadAllText("LocalInput.xml");

var conf = new ApplicationRequestInput(){
    Certificate = LocalSettings.ProdCertificatePem,
    PrivateKey = LocalSettings.ProdPrivkeyPem,
    SignerId = "1234567890",
    SenderId = "1234567890",
    RequestId = "ffffffff-ffff-ffff-ffff-ffffffff",
    FileContent = inputContent,
    FileType = UploadFileTypes.NDCAPXMLI
};

var xmlf = ApplicationRequests.UploadFile(conf);
File.WriteAllText("LocalOutput.xml", xmlf.XmlData);
