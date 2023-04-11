﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Newtonsoft.Json.Linq;
using Environment = Frends.HIT.FinancialServices.Nordea.Helpers.Enums.Environment;
using Status = Frends.HIT.FinancialServices.Nordea.Helpers.Enums.Status;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Frends.HIT.FinancialServices.Nordea.Definitions;
using Frends.HIT.FinancialServices.Nordea.Helpers;
using Frends.HIT.FinancialServices.Nordea.Services;

#pragma warning disable 1591

namespace Frends.HIT.FinancialServices.Nordea
{
    public class WebServices
    {
        /// <summary>
        /// Fetches information on authorized user file types and service IDs. 
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>JToken. Properties: FileReference, TargetId</returns>
        public static async Task<JToken> GetUserInfo(GetUserInfoInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string url = input.Url;
            string certificate = input.Certificate;

            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId),
            };

            X509Certificate2 cert = CertificateService.GetX509Certificate(certificate);

            Validators.ValidateParameters(url, certificate, environment, stringParameters);

            var env = (Enums.Environment)Enum.Parse(typeof(Enums.Environment), environment);
            

            var message = MessageService.GetUserInfoMessage(cert, customerId, input.TargetId, env, input.RequestId);
            var result = await WebService.CallWebService(url, message, MessageService.SoftwareId, input.ConnectionTimeOutSeconds, cancellationToken);
            Debug.WriteLine(result.StatusCode.ToString());
            string resultXml = result.Body;
            var applicationResponse = CheckResultForErrorsAndReturnApplicationResult(resultXml);

            return Helper.GetUserInfoFromResponseXml(applicationResponse);
        }

        /// <summary>
        /// DownloadFileList downloads a list of files available for download from Nordea.
        /// Files can be filtered with different parameters: FileType, StartDate, EndDate or Status.
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>JToken array. Properties: FileReference, TargetId, ParentFileReference, FileType, FileTimestamp, Status</returns>
        public static async Task<JToken> DownloadFileList(FileListInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string status = input.Status;
            string url = input.Url;
            string certificate = input.Certificate;

            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId)
            };

            // Fetches certificate based on issuer
            X509Certificate2 cert = CertificateService.GetX509Certificate(certificate);

            // Validate parameters
            Validators.ValidateParameters(url, certificate, environment, stringParameters);

            if (!string.IsNullOrEmpty(status))
            {
                Validators.ValidateStatusParameter(status);
            }

            var env = (Enums.Environment)Enum.Parse(typeof(Enums.Environment), environment);
            var fileStatus = string.IsNullOrEmpty(status) ? Enums.Status.ALL : (Enums.Status)Enum.Parse(typeof(Enums.Status), status);
            var startDateParam = input.StartDate.ResolveDate();
            var endDateParam = input.EndDate.ResolveDate();

            var message = MessageService.GetDownloadFileListMessage(cert, customerId, input.FileType, input.TargetId, startDateParam, endDateParam, fileStatus, env, input.RequestId);
            var result = await WebService.CallWebService(url, message, MessageService.SoftwareId, input.ConnectionTimeOutSeconds, cancellationToken);
            string resultXml = result.Body;
            var applicationResponse = CheckResultForErrorsAndReturnApplicationResult(resultXml);

            return Helper.GetFileListResultFromResponseXml(applicationResponse);
        }

        /// <summary>
        /// Uploads a file to Nordea. 
        /// File type needs to be specified for the file.
        /// File content is compressed by GZIP.
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>JToken. Properties: CustomerId, Timestamp, ResponseCode, Encrypted, Compressed, AmountTotal, TransactionCount</returns>
        public static async Task<JToken> UploadFile(UploadFileInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string fileInput = input.FileInput;
            string fileType = input.FileType;
            string url = input.Url;
            string certificate = input.Certificate;

            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId),
                new KeyValuePair<string, string>("fileInput", fileInput),
                new KeyValuePair<string, string>("fileType", fileType)
            };

            X509Certificate2 cert = CertificateService.GetX509Certificate(certificate);

            Validators.ValidateParameters(url, certificate, environment, stringParameters);

            var env = (Enums.Environment)Enum.Parse(typeof(Enums.Environment), environment);

            var encoding = string.IsNullOrEmpty(input.FileEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(input.FileEncoding);

            var message = MessageService.GetUploadFileMessage(cert, customerId, env, input.RequestId, fileInput, fileType, input.TargetId, encoding);
            var result = await WebService.CallWebService(url, message, MessageService.SoftwareId, input.ConnectionTimeOutSeconds, cancellationToken);
            string resultXml = result.Body;
            var applicationResponse = CheckResultForErrorsAndReturnApplicationResult(resultXml);

            return Helper.GetFileInfoFromResponseXml(applicationResponse);
        }

        /// <summary>
        /// Downloads a file from web service.
        /// In case of an error an exception is thrown.
        /// </summary>
        /// <returns>Returns the downloaded file content</returns>
        public static DownLoadFileOutput DownloadFile(DownLoadFileInput input, CancellationToken cancellationToken)
        {
            string customerId = input.CustomerId;
            string environment = input.Environment;
            string fileEncoding = input.FileEncoding;
            string fileReference = input.FileReference;
            string status = input.Status;
            string url = input.Url;
            string certificate = input.Certificate;

            var stringParameters = new[]
            {
                new KeyValuePair<string, string>("customerId", customerId),
                new KeyValuePair<string, string>("fileReference", fileReference)
            };

            if (!string.IsNullOrEmpty(status))
            {
                Validators.ValidateStatusParameter(status);
            }

            X509Certificate2 cert = CertificateService.GetX509Certificate(certificate);

            Validators.ValidateParameters(url, certificate, environment, stringParameters);

            var env = (Enums.Environment)Enum.Parse(typeof(Enums.Environment), environment);
            var fileStatus = string.IsNullOrEmpty(status) ? Enums.Status.ALL : (Enums.Status)Enum.Parse(typeof(Enums.Status), status);
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