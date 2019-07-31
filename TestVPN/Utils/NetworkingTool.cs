using System.Linq;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace TestVPN.Utils
{
    class NetworkingTool
    {
        public static string GetLocalIp()
        {
            foreach (HostName localHostName in NetworkInformation.GetHostNames())
            {
                if (localHostName.IPInformation != null)
                {
                    if (localHostName.Type == HostNameType.Ipv4)
                    {
                        return localHostName.ToString();
                    }
                }
            }

            return "Unknown";
        }
    }
}
