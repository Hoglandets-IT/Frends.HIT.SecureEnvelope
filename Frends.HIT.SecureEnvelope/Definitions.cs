using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Frends.HIT.SecureEnvelope.Definitions {
    /// <summary>
    /// Parameters for creating an ApplicationRequest
    /// </summary>
    public class ApplicationRequestInput {
        /// <summary>
        /// The certificate, base64-encoded Pem
        /// An Exception is thrown if a certificate is not found or it has already expired.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Expression")]
        public string Certificate { get; set; }
        
        /// <summary>
        /// The private key, base64-encoded PEM
        /// An Exception is thrown if a certificate is not found or it has already expired.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Expression")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// Sender ID number. Identifier for the company
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Expression")]
        [DefaultValue("")]
        public string SenderId { get; set; }
        
        /// <summary>
        /// Signer ID number. The certificate needs to be assigned to the same customer id (e.g. "1234567890").
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Expression")]
        [DefaultValue("")]
        public string SignerId { get; set; }

        /// <summary>
        /// Request Id is the HIT identifier for the request in question
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Expression")]
        public string RequestId { get; set; }

        /// <summary>
        /// The file to include in the Application Request
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Expression")]
        public string FileContent { get; set; }

        /// <summary>
        /// File type to upload (e.g. "pain.001.001.03").
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue(UploadFileTypes.NDCAPXMLI)]
        public UploadFileTypes FileType { get; set; }
    }

    /// <summary>
    /// The response back from the UploadFile task
    /// </summary>
    public class ApplicationRequestOutput {
        /// <summary>
        /// The minified and signed XML data to send to Nordea
        /// </summary>
        public string XmlData { get; set; }

        /// <summary>
        /// The application request object, to retrieve any signature parameters
        /// </summary>
        public UploadFileApplicationRequest ApplicationRequest { get; set; }
    }
   
    /// <summary>
    /// The available types for uploading files to Nordea
    /// </summary>
    public enum UploadFileTypes {
        
        /// The XML file type for pain.001.001.03
        [Display(Name = "pain.001.001.03")]
        NDCAPXMLI,

        /// The XML file type for camt.055.001.01
        [Display(Name = "camt.055.001.01")]
        NDCAPCANXMLI
    }

    /// <summary>
    /// The request document for uploading a file
    /// </summary>
    [XmlRoot(ElementName = "ApplicationRequest", Namespace = "http://bxd.fi/xmldata/")]
    public class UploadFileApplicationRequest {
        /// <summary>
        /// "SenderID"
        /// Code used by the bank to identify the customer who originated this request. This code is 
        /// bank specific, i.e. each bank issues and manages its own CustomerIds. 
        /// When signing the ApplicationRequest element, the certificate used to verify the Signature must be 
        /// associated with the CustomerId given in this field
        /// </summary>
        [XmlElement(ElementName = "CustomerId")]
        public string CustomerId { get; set; }

        /// <summary>
        /// The request type for the application request, always UploadFile for file uploads
        /// </summary>
        [XmlElement(ElementName = "Command")]
        public virtual string Command { get { return "UploadFile"; } set {} }
       
        /// <summary>
        /// The timestamp when the document was created
        /// </summary>
        [XmlElement(ElementName = "Timestamp")]
        public DateTime Timestamp { get; set; }


        /// <summary>
        /// The environment to send the file to
        /// </summary>
        [XmlElement(ElementName = "Environment")]
        public virtual string Environment { get { return "PRODUCTION"; } set {} }

        /// <summary>
        /// "SignerID"
        /// The SignerId for the customer
        /// </summary>
        [XmlElement(ElementName = "TargetId")]
        public string TargetId { get; set; }

        /// <summary>
        /// A custom unique identifier for the sent file
        /// </summary>
        /// <value></value>
        [XmlElement(ElementName = "ExecutionSerial")]
        public string ExecutionSerial { get; set; }

        /// <summary>
        /// Indicate the content of the file is compressed
        /// </summary>
        [XmlElement(ElementName = "Compression")]
        public virtual string Compression { get { return "false"; } set {} }

        /// <summary>
        /// Indicate the compression method used to compress the file
        /// </summary>
        [XmlElement(ElementName = "CompressionMethod")]
        public virtual string CompressionMethod { get { return "GZIP"; } set {} }
        /// <summary>
        /// The identifier of the client sending the data
        /// </summary>
        [XmlElement(ElementName = "SoftwareId")]
        public virtual string SoftwareId { get { return "Frends-IntegrationPlatform"; } set {} }

        /// <summary>
        /// The type of file (e.g. pain.001.001.04)
        /// </summary>
        /// <value></value>
        [XmlElement(ElementName = "FileType")]
        public UploadFileTypes FileType { get; set; }

        /// <summary>
        /// The content, gzipped and Base64-encoded file
        /// </summary>
        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// The signature of the data
        /// </summary>
        /// <value></value>
        [XmlElement(ElementName = "Signature")]
        internal Signature Signature { get; set; }

        private string _XMLDATA { get; set; }

        /// <summary>
        /// Create a new Application Request
        /// </summary>
        /// <param name="customerId">The ID of the customer (SenderID)</param>
        /// <param name="targetId">The ID of the signer/certificate (SignerID)</param>
        /// <param name="executionSerial">Custom serial number to pass in the object</param>
        /// <param name="fileType">The type of file being sent</param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static UploadFileApplicationRequest New(
            string customerId,
            string targetId,
            string executionSerial,
            UploadFileTypes fileType,
            string fileContent
        ) {
            return new UploadFileApplicationRequest(){
                CustomerId = customerId,
                TargetId = targetId,
                ExecutionSerial = executionSerial,
                Timestamp = DateTime.Now,
                FileType = fileType,
                // Content = Helpers.GzipAndBase64Encode(fileContent),
                Content = Helpers.Base64Encode(fileContent),
            };
        }

        /// <summary>
        /// Serialize the Application Request for transfer to bank
        /// </summary>
        public void Serialize() {
            XmlSerializer serializer = new XmlSerializer(typeof(UploadFileApplicationRequest));

            using (var sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings{
                Encoding = Encoding.UTF8,
                Indent = false,
                OmitXmlDeclaration = true
            })) {
                serializer.Serialize(writer, this);
                _XMLDATA = sw.ToString();
            }
        }

        /// <summary>
        /// Sign and serialize the ApplicationRequest for transfer to bank
        /// </summary>
        /// <param name="cert"></param>
        public void Sign(X509Certificate2 cert) {
            Serialize();

            XmlDocument xmlDoc = new(){
                PreserveWhitespace = false
            };

            xmlDoc.LoadXml(_XMLDATA);

            RSA rsaKey = cert.GetRSAPrivateKey();

            SignedXml signedXml = new SignedXml(xmlDoc);
            
            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data(cert);
            keyInfo.AddClause( keyInfoData );
            signedXml.KeyInfo = keyInfo;
            signedXml.SigningKey = rsaKey;

            Reference reference = new Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);

            signedXml.ComputeSignature();

            XmlElement xmlDigitalSignature = signedXml.GetXml();

            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

            _XMLDATA = xmlDoc.OuterXml;
        }

        /// <summary>
        /// Retrieve signed data
        /// </summary>
        /// <returns></returns>
        public string GetSigned() {
            return _XMLDATA;
        }
    }

    public class SignedXmlWithId : SignedXml
    {
        public SignedXmlWithId(XmlDocument document) : base(document)
        {
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            var idElem = base.GetIdElement(doc, id);

            if (idElem == null)
            {
                var nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
                idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
            }

            return idElem;
        }
    }
    
    public class FileDescriptor {
        public string FileReference { get; set; }
        public string TargetId { get; set; }
        public string ParentFileReference { get; set; }
        public string FileType { get; set; }
        public string FileTimestamp { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// The response document from Nordea
    /// </summary>
    [XmlRoot(ElementName = "ApplicationResponse", Namespace = "http://bxd.fi/xmldata/")]
    public class ApplicationResponse {
        /// <summary>
        /// "SenderID"
        /// Code used by the bank to identify the customer who originated this request. This code is 
        /// bank specific, i.e. each bank issues and manages its own CustomerIds. 
        /// When signing the ApplicationRequest element, the certificate used to verify the Signature must be 
        /// associated with the CustomerId given in this field
        /// </summary>
        [XmlElement(ElementName = "CustomerId")]
        public string CustomerId { get; set; }    

        public string Timestamp { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public string ExecutionSerial { get; set; }
        public bool Encrypted { get; set; }
        public bool Compressed { get; set; }
        public string CompressionMethod { get; set; }
        public FileDescriptor FileDescriptors { get; set; }
        public string FileType { get; set; }
        public string Content { get; set; }

        public static ApplicationResponse Deserialize(string xml) {
            // var document = new XmlDocument() { PreserveWhitespace = true };
            // document.LoadXml(xml);

            // var signedXml = new SignedXml(document);

            // var signatureElement = document.GetElementsByTagName("ds:Signature").Cast<XmlElement>().First();
            // signedXml.LoadXml(signatureElement);

            // if (!signedXml.CheckSignature()) {
            //     throw new CryptographicException("Signature verification failed");
            // }

            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationResponse));
            using (var sr = new StringReader(xml))
            using (XmlReader reader = XmlReader.Create(sr)) {
                var appResponse = (ApplicationResponse)serializer.Deserialize(reader);
                // appResponse.Content = Helpers.Base64Decode(appResponse.Content);
                appResponse.Content = Helpers.GetContentFromBase64(
                    appResponse.Content,
                    appResponse.Compressed,
                    Encoding.UTF8
                );

                // // Try gunzip
                // if (appResponse.Compressed && appResponse.CompressionMethod == "GZIP") {
                //     appResponse.Content = Helpers.Gunzip(appResponse.Content);                    
                // }


                return appResponse;
            }
        }
    }

}
