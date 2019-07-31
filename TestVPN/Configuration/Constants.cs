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

        public static readonly string lastServerPropertyName = "last server";

        public static readonly string connectionProfileName = "UWPTestVPN";
    }
}
