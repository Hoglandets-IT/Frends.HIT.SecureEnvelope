using System.Security.Cryptography.Xml;
using System.Xml;

#pragma warning disable 1591

namespace Frends.Community.PaymentServices.Nordea.Helpers
{
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
}
