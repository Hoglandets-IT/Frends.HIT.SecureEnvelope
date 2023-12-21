using System.Text;
using System.Xml.Linq;

namespace Frends.HIT.SecureEnvelope {
    
    /// <summary>
    /// Handle creating and decoding Application Requests
    /// </summary>
    public class ApplicationRequests {
        
        /// <summary>
        /// Upload a file to Nordea 
        /// </summary>
        /// <param name="input">Input parameters and file content</param>
        public static Definitions.ApplicationRequestOutput UploadFile(Definitions.ApplicationRequestInput input) {
            var cert = Helpers.GetX509Certificate(input.Certificate, input.PrivateKey);

            var applicationRequest = Definitions.UploadFileApplicationRequest.New(
                customerId: input.SenderId,
                targetId: input.SignerId,
                executionSerial: input.RequestId,
                fileType: input.FileType,
                fileContent: input.FileContent
            );

            applicationRequest.Sign(cert);

            var response = new Definitions.ApplicationRequestOutput(){
                XmlData = applicationRequest.GetSigned(),
                ApplicationRequest = applicationRequest
            };


            return response;
        }
    }
}