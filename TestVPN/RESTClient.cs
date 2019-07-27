using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace TestVPN
{
    class JWTRESTClient
    {
        public JWTRESTClient()
        {
            this.client = new HttpClient();
        }

        public async Task Request(string url, string auth)
        {
            string jtwTocken = JWTBuilder.Build(payload, auth);
            this.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jtwTocken);

            HttpResponseMessage response = await this.client.GetAsync(url);

            HttpContent content = response.Content;

            resultJSON = await content.ReadAsStringAsync();
        }

        public static ConnectionConfiguration ReadToObject(string json)
        {
            var deserialized = new ConnectionConfiguration();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(deserialized.GetType());
            deserialized = ser.ReadObject(ms) as ConnectionConfiguration;
            return deserialized;
        }

        private string payload = "{\"name\":\"John Doe\"}";

        private HttpClient client;

        private string resultJSON;
        
        public string ResultJSON { get => resultJSON; set => resultJSON = value; }
    }
}
