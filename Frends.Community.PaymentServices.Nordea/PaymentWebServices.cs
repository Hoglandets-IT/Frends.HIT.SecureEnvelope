using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Newtonsoft.Json.Linq;
using Frends.Community.PaymentServices.Nordea.Definitions;
using Frends.Community.PaymentServices.Nordea.Helpers;
using Frends.Community.PaymentServices.Nordea.Services;
using Environment = Frends.Community.PaymentServices.Nordea.Helpers.Enums.Environment;
using Status = Frends.Community.PaymentServices.Nordea.Helpers.Enums.Status;

#pragma warning disable 1591

namespace Frends.Community.PaymentServices.Nordea
{
    public class WebServices
    {
        /// <summary>
        /// Fetches user info from the web service. 
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>JToken. Properties: FileReference, TargetId</returns>
        public static JToken GetUserInfo(GetUserInfoInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string url = input.Url;

            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId),
            };

            X509Certificate2 cert = CertificateService.FindCertificate(input.CertificateIssuedBy);

            Validators.ValidateParameters(url, cert, environment, stringParameters);

            var env = (Environment)Enum.Parse(typeof(Environment), environment);

            var message = MessageService.GetUserInfoMessage(cert, customerId, input.TargetId, env, input.RequestId);
            var result = WebService.CallWebService(url, message, MessageService.SoftwareId, input.ConnectionTimeOutSeconds, cancellationToken);
            string resultXml = result.Result.Body;
            var applicationResponse = CheckResultForErrorsAndReturnApplicationResult(resultXml);

            return Helper.GetUserInfoFromResponseXml(applicationResponse);
        }

        /// <summary>
        /// DownloadFileList fetches a filelist from the web service. 
        /// Files can be filtered with different parameters: FileType, StartDate, EndDate or Status.
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>JToken array. Properties: FileReference, TargetId, ParentFileReference, FileType, FileTimestamp, Status</returns>
        public static JToken DownloadFileList(FileListInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string status = input.Status;
            string url = input.Url;

            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId)
            };

            // Fetches certificate based on issuer
            X509Certificate2 cert = CertificateService.FindCertificate(input.CertificateIssuedBy);

            // Validate parameters
            Validators.ValidateParameters(url, cert, environment, stringParameters);

            if (!string.IsNullOrEmpty(status))
            {
                Validators.ValidateStatusParameter(status);
            }

            var env = (Environment)Enum.Parse(typeof(Environment), environment);
            var fileStatus = string.IsNullOrEmpty(status) ? Status.ALL : (Status)Enum.Parse(typeof(Status), status);
            var startDateParam = input.StartDate.ResolveDate();
            var endDateParam = input.EndDate.ResolveDate();

            var message = MessageService.GetDownloadFileListMessage(cert, customerId, input.FileType, input.TargetId, startDateParam, endDateParam, fileStatus, env, input.RequestId);
            var result = WebService.CallWebService(url, message, MessageService.SoftwareId, input.ConnectionTimeOutSeconds, cancellationToken);
            string resultXml = result.Result.Body;
            var applicationResponse = CheckResultForErrorsAndReturnApplicationResult(resultXml);

            return Helper.GetFileListResultFromResponseXml(applicationResponse);
        }

        /// <summary>
        /// Uploads a file to the web service. 
        /// File type needs to be specified for the file.
        /// File content is compressed by GZIP.
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>JToken. Properties: FileReference, TargetId, ParentFileReference, FileType, FileTimestamp, Status</returns>
        public static JToken UploadFile(UploadFileInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string fileInput = input.FileInput;
            string fileType = input.FileType;
            string url = input.Url;

            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId),
                new KeyValuePair<string, string>("fileInput", fileInput),
                new KeyValuePair<string, string>("fileType", fileType)
            };

            X509Certificate2 cert = CertificateService.FindCertificate(input.CertificateIssuedBy);

            Validators.ValidateParameters(url, cert, environment, stringParameters);

            var env = (Environment)Enum.Parse(typeof(Environment), environment);
            
            var encoding = string.IsNullOrEmpty(input.FileEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(input.FileEncoding);

            var message = MessageService.GetUploadFileMessage(cert, customerId, env, input.RequestId, fileInput, fileType, input.TargetId, encoding);
            var result = WebService.CallWebService(url, message, MessageService.SoftwareId, input.ConnectionTimeOutSeconds, cancellationToken);
            string resultXml = result.Result.Body;
            var applicationResponse = CheckResultForErrorsAndReturnApplicationResult(resultXml);

            return Helper.GetFileInfoFromResponseXml(applicationResponse);
        }

        /// <summary>
        /// Downloads a file from web service.
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>Returns the downloaded file content</returns>
        public static DownLoadFileOutput DownloadFile(DownloadFileInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string fileEncoding = input.FileEncoding;
            string fileReference = input.FileReference;
            string status = input.Status;
            string url = input.Url;
            
            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId),
                new KeyValuePair<string, string>("fileReference", fileReference)
            };

            if (!string.IsNullOrEmpty(status))
            {
                Validators.ValidateStatusParameter(status);
            }

            X509Certificate2 cert = CertificateService.FindCertificate(input.CertificateIssuedBy);

            Validators.ValidateParameters(url, cert, environment, stringParameters);

            var env = (Environment)Enum.Parse(typeof(Environment), environment);
            var fileStatus = string.IsNullOrEmpty(status) ? Status.ALL : (Status)Enum.Parse(typeof(Status), status);
            var encoding = string.IsNullOrEmpty(fileEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(fileEncoding);

            var message = MessageService.GetDownloadFileMessage(cert, customerId, input.FileType, input.TargetId, env, input.RequestId, fileStatus, fileReference);
            var result = WebService.CallWebService(url, message, MessageService.SoftwareId, input.ConnectionTimeOutSeconds, cancellationToken);
            var resultXml = result.Result.Body;
            var applicationResponse = CheckResultForErrorsAndReturnApplicationResult(resultXml);

            return new DownLoadFileOutput { FileContent = Helper.GetFileFromResponseXml(applicationResponse, encoding) };
        }

        private static string CheckResultForErrorsAndReturnApplicationResult(string resultXml)
        {
            var applicationResponse = Helper.GetApplicationResponseXml(resultXml);

            if (string.IsNullOrEmpty(applicationResponse))
            {
                throw new Exception($"Server returned: {resultXml}");
            }
            if (!Helper.CheckIfCallWasSuccessful(resultXml))
            {
                throw new Exception(Helper.GetErrorMessage(applicationResponse));
            }
            // soap envelope signature is not checked as .Net does not understand BinarySecurityToken certificate
            if (!Helper.VerifyApplicationResponseSignature(applicationResponse))
            {
                throw new Exception($"Application response signature does not match to content! Response: {applicationResponse}");
            }

            return applicationResponse;
        }
    }
}
