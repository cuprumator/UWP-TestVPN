using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using TestVPN.RESTClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Storage.Streams;
using TestVPN.Utils;

namespace TestVPN.Configuration
{
    class ConfigurationManager
    {
        public static void Load(IRESTClient client, string url)
        {
            Task<string> response = client.Request(url);
            response.Wait();

            conf = Decerealize(response.Result);
            //protectedJson = await Protector.Protect(response);
        }

        private static ConnectionConfiguration Decerealize(string json)
        {
            var deserialized = new ConnectionConfiguration();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(json.GetType());

            ConnectionConfiguration obj = ser.ReadObject(ms) as ConnectionConfiguration;

            return obj;
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
