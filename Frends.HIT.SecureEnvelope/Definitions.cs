using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;
using System.CodeDom.Compiler;
using System.Xml.Schema;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Frends.HIT.SecureEnvelope.Definitions {
    public enum Environment
    {
        TEST,
        PRODUCTION
    }

    /// <summary>
    /// Parameters for creating an ApplicationRequest
    /// </summary>
    public class ApplicationRequestInput {
        /// <summary>
        /// The certificate, base64-encoded Pem
        /// An Exception is thrown if a certificate is not found or it has already expired.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        public string Certificate { get; set; }
        
        /// <summary>
        /// The private key, base64-encoded PEM
        /// An Exception is thrown if a certificate is not found or it has already expired.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
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
        [DisplayFormat(DataFormatString = "Text")]
        public string FileContent { get; set; }

        /// <summary>
        /// File type to upload (e.g. "pain.001.001.02").
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("")]
        public string FileType { get; set; }
    }


     internal class XmlSettings {
            internal static XNamespace ModelNs = "http://model.bxd.fi";
            internal static readonly XNamespace CorporateNs = "http://bxd.fi/CorporateFileService";
            internal static readonly XNamespace DataNs = "http://bxd.fi/xmldata/";
            internal static readonly XNamespace EnvNs = "http://schemas.xmlsoap.org/soap/envelope/";
            internal static readonly XNamespace WsuNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
            internal static readonly XNamespace WsseNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

            internal static readonly Dictionary<string, XNamespace> Namespaces = new Dictionary<string, XNamespace>
                                                                    {
                                                                        { "mod", ModelNs },
                                                                        { "cor", CorporateNs },
                                                                        { "bxd", DataNs },
                                                                        { "soapenv", EnvNs },
                                                                        { "wsu", WsuNs },
                                                                        { "wsse", WsseNs },
                                                                    };


            internal static readonly Dictionary<string, XNamespace> FewNamespaces = new Dictionary<string, XNamespace>
                                                                    {
                                                                        { "soapenv", EnvNs }
                                                                    };

    }

    
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ApplicationRequest", Namespace = "http://bxd.fi/xmldata/")]
    /// <summary>
    /// The request document for uploading a file
    /// </summary>
    public class UploadFileApplicationRequest {
        /// <summary>
        /// "SenderID"
        /// Code used by the bank to identify the customer who originated this request. This code is 
        /// bank specific, i.e. each bank issues and manages its own CustomerIds. 
        /// When signing the ApplicationRequest element, the certificate used to verify the Signature must be 
        /// associated with the CustomerId given in this field
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "CustomerId")]
        public string CustomerId { get; set; }

        /// <summary>
        /// The request type for the application request, always UploadFile for file uploads
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "Command")]
        public virtual string Command { get { return "UploadFile"; } set {} }
       
        /// <summary>
        /// The timestamp when the document was created
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "Timestamp")]
        public DateTime Timestamp { get; set; }


        /// <summary>
        /// The environment to send the file to
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "Environment")]
        public virtual string Environment { get { return "PRODUCTION"; } set {} }

        /// <summary>
        /// "SignerID"
        /// The SignerId for the customer
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "TargetId")]
        public string TargetId { get; set; }

        /// <summary>
        /// A custom unique identifier for the sent file
        /// </summary>
        /// <value></value>
        [System.Xml.Serialization.XmlElement(ElementName = "ExecutionSerial")]
        public string ExecutionSerial { get; set; }

        /// <summary>
        /// Indicate the content of the file is compressed
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "Compression")]
        public virtual string Compression { get { return "true"; } set {} }

        /// <summary>
        /// Indicate the compression method used to compress the file
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "CompressionMethod")]
        public virtual string CompressionMethod { get { return "GZIP"; } set {} }
        /// <summary>
        /// The identifier of the client sending the data
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "SoftwareId")]
        public virtual string SoftwareId { get { return "Frends-IntegrationPlatform"; } set {} }

        /// <summary>
        /// The type of file (e.g. pain.001.001.04)
        /// </summary>
        /// <value></value>
        [System.Xml.Serialization.XmlElement(ElementName = "FileType")]
        public string FileType { get; set; }

        /// <summary>
        /// The content, gzipped and Base64-encoded file
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// The signature of the data
        /// </summary>
        /// <value></value>
        [System.Xml.Serialization.XmlElement(ElementName = "Signature")]
        internal Signature Signature { get; set; }

        private string _XMLDATA { get; set; }

        public static UploadFileApplicationRequest New(
            string customerId,
            string targetId,
            string executionSerial,
            string fileType,
            string fileContent
        ) {
            return new UploadFileApplicationRequest(){
                CustomerId = customerId,
                TargetId = targetId,
                ExecutionSerial = executionSerial,
                Timestamp = DateTime.Now,
                FileType = fileType,
                Content = Helpers.GetBase64String(fileContent),
            };
        }

        public void Serialize() {
            XmlSerializer serializer = new XmlSerializer(typeof(UploadFileApplicationRequest));

            using (var sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings{
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = true
            })) {
                serializer.Serialize(writer, this);
                _XMLDATA = sw.ToString();
            }
        }

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

        public string GetSigned() {
            return _XMLDATA;
        }

       

    }
}