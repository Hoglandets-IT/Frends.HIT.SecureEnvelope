using System.Text;
using System.Xml.Linq;

namespace Frends.HIT.SecureEnvelope {
    
    /// <summary>
    /// Handle creating and decoding Application Requests
    /// </summary>
    public class ApplicationRequests {
        
        public static string UploadFile(Definitions.ApplicationRequestInput input) {
            var cert = Helpers.GetX509Certificate(input.Certificate, input.PrivateKey);

            var applicationRequest = Definitions.UploadFileApplicationRequest.New(
                customerId: input.SenderId,
                targetId: input.SignerId,
                executionSerial: input.RequestId,
                fileType: input.FileType,
                fileContent: input.FileContent
            );

            applicationRequest.Sign(cert);
            return applicationRequest.GetSigned();

            // var applicationRequest = new Definitions.ApplicationRequest(){
            //     CustomerId = input.CustomerId,
            //     Environment = Definitions.Environment.PRODUCTION,
            //     FileContent = input.FileContent,
            //     FileType = input.FileType
            // };

            // applicationRequest.CreateApplicationRequestMessage();


            // return new Definitions.ApplicationRequest();
        }
    }
}