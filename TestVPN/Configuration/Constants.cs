using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVPN.Configuration
{
    class Constants
    {
        public static readonly string[] configurationHosts =
        {
            "http://159.65.72.139.sslip.io/api/list.json",
            "http://159.65.72.139.sslip.io/api/servers.json"
        };

        public static readonly string secret = "jVnPSec";
    }
}
