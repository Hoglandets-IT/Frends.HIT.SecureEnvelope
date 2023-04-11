﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.HIT.FinancialServices.Nordea.Definitions
{
    public class DownLoadFileInput
    {
        /// <summary>
        /// Url of the web service (e.g. "http://filetransfer.test.nordea.com/services/CorporateFileService").
        /// Note that test environment is available only when Corporate eGateway service is used.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue(@"http://filetransfer.test.nordea.com/services/CorporateFileService")]
        public string Url { get; set; }

        /// <summary>
        /// The cert in string format.
        /// An Exception is thrown if a certificate is not found or it has already expired.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("")]
        public string Certificate { get; set; }

        /// <summary>
        /// The issuer of the Base-64 encoded X.509 to be used for signing web service calls. First matching certificate is used.
        /// An Exception is thrown if a certificate is not found or it has already expired.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("")]
        public string CertificateIssuedBy { get; set; }

        /// <summary>
        /// Environment to be used for the web service. Valid values are "TEST" or "PRODUCTION".
        /// When testing against production, this has to be "TEST" or the files will be processed as
        /// actual production files.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"TEST\"")]
        public string Environment { get; set; }

        /// <summary>
        /// Customer Id number. The certificate needs to be assigned to the same customer id (e.g. "1234567890").
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"\"")]
        public string CustomerId { get; set; }

        /// <summary>
        /// Request Id is the web service call id number which needs to be unique for at least 3 months.
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int RequestId { get; set; }

        /// <summary>
        /// File reference id to be downloaded (e.g. "123456").
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"\"")]
        public string FileReference { get; set; }

        /// <summary>
        /// Encoding of the file to be download, if empty UTF8 will be used.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"utf-8\"")]
        public string FileEncoding { get; set; }

        /// <summary>
        /// Optional parameter for filtering downloaded files (e.g. "pain.001.001.02").
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"\"")]
        public string FileType { get; set; }

        /// <summary>
        /// The logical folder name where the file(s) of the customer are stored in the bank. A user can have access to several folders.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"\"")]
        public string TargetId { get; set; }

        /// <summary>
        /// Optional parameter Status can be used to filter filelist.
        /// Valid values for are "NEW", "DOWNLOADED" and "ALL" (NEW = files not downloaded yet, DOWNLOADED = files already downloaded, ALL = fetch all available files).
        /// If no parameter is given or if the status is "ALL", all files will be listed.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"ALL\"")]
        public string Status { get; set; }

        /// <summary>
        /// Timeout in seconds to be used for the connection and operation.
        /// </summary>
        [DefaultValue(30)]
        public int ConnectionTimeOutSeconds { get; set; }
    }
}
