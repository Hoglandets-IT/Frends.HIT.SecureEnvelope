using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.HIT.FinancialServices.Nordea.Helpers
{

#pragma warning disable 1591

    public class Enums
    {
        public enum Environment
        {
            TEST,
            PRODUCTION
        }
        // Possible statuses for files
        // NEW = New (not yet downloaded)
        // DOWNLOADED = Downloaded
        // ALL = Both new and downloaded files will be listed
        public enum Status
        {
            NEW,
            DOWNLOADED,
            ALL
        }
    }
}
