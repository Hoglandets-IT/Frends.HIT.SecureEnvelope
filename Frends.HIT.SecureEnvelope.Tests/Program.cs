using Frends.HIT.SecureEnvelope;
using Frends.HIT.SecureEnvelope.Definitions;
using Frends.HIT.SecureEnvelope.Tests;

// Open and read LocalInput.xml
var inputContent = File.ReadAllText("LocalInput.xml");

var conf = new ApplicationRequestInput(){
    Certificate = LocalSettings.ProdCertificatePem,
    PrivateKey = LocalSettings.ProdPrivkeyPem,
    SignerId = "17071828802",
    SenderId = "67818244775",
    RequestId = "ffffffff-ffff-ffff-ffff-ffffffff",
    FileContent = inputContent,
    // FileType = "pain.001.001.03"
    FileType = "NDCAPXMLI"
};

var xmlf = ApplicationRequests.UploadFile(conf);
File.WriteAllText("LocalOutput.xml", xmlf);
