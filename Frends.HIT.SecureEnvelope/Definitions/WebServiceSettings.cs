using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.HIT.SecureEnvelope.Definitions
{
    internal class WebServiceSettings
    {
        public string Authentication = "WindowsIntegratedSecurity";
        public bool FollowRedirects = false;
        public bool AllowInvalidCertificate = false;
        public bool AllowInvalidResponseContentTypeCharSet = false;
        public bool ThrowExceptionOnErrorResponse = true;
    }
}
