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
        public static async Task LoadAsync(IRESTClient client, string url)
        {
            string response = await client.Request(url);

            ProtectedJson = await Protector.Protect(response);
        }

        private static ConnectionConfiguration Decerealize(string json)
        {
            ConnectionConfiguration data = JsonConvert.DeserializeObject<ConnectionConfiguration>(json);
            return data;
        }

        public static async Task<ConnectionConfiguration> GetUnprotecedAsync()
        {
            string json = await Protector.Unprotect(ProtectedJson);
            return Decerealize(json);
        }
        
        private static IBuffer ProtectedJson;

        public static async Task<List<Server>> GetServers()
        {
            var conf = await GetUnprotecedAsync();
            return conf.servers;
        }
    }
}
