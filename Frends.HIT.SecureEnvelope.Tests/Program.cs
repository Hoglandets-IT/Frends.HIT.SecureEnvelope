using Frends.HIT.SecureEnvelope;
using Frends.HIT.SecureEnvelope.Definitions;
using Frends.HIT.SecureEnvelope.Tests;

// Open and read LocalInput.xml
// var inputContent = File.ReadAllText("LocalInput.xml");

// var conf = new ApplicationRequestInput(){
//     Certificate = LocalSettings.ProdCertificatePem,
//     PrivateKey = LocalSettings.ProdPrivkeyPem,
//     SignerId = "17071828802",
//     SenderId = "67818244775",
//     RequestId = "ffffffff-ffff-ffff-ffff-ffffffff",
//     FileContent = inputContent,
//     FileType = UploadFileTypes.NDCAPXMLI
// };

// var xmlf = ApplicationRequests.UploadFile(conf);
// File.WriteAllText("/mnt/d/Temp/Nordea/TestFiles/100K95ACT.xml", xmlf.XmlData);

var inputData = File.ReadAllText("LocalResponse1.xml");

var appr = ApplicationResponse.Deserialize(inputData);

Console.WriteLine(appr);