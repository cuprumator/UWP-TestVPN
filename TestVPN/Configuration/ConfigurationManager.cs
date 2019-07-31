using TestVPN.RESTClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Storage.Streams;
using TestVPN.Utils;
using Newtonsoft.Json;

namespace TestVPN.Configuration
{
    class ConfigurationManager
    {
        public static async Task Load(IRESTClient client, string url)
        {
            string response = await client.Request(url);

            protectedJson = await Protector.Protect(response);
        }

        private static ConnectionConfiguration Decerealize(string json)
        {
            ConnectionConfiguration data = JsonConvert.DeserializeObject<ConnectionConfiguration>(json);
            return data;
        }

        public static async Task<ConnectionConfiguration> GetUnproteced()
        {
            string json = await Protector.Unprotect(protectedJson);
            return Decerealize(json);
        }
        
        private static IBuffer protectedJson;
        private static ConnectionConfiguration conf;

        public static async Task<List<Server>> GetServers()
        {
            var conf = await GetUnproteced();
            return conf.servers;
        }
    }
}
