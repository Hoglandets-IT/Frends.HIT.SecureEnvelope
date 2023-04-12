using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.HIT.SecureEnvelope.Definitions
{
    internal class HttpResponse
    {
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public int StatusCode { get; set; }
    }
}
