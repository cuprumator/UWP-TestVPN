using System.Net.Http;
using System.Threading.Tasks;
using TestVPN.Utils;

namespace TestVPN.RESTClient
{
    class JWTRESTClient : IRESTClient
    {
        public JWTRESTClient(string secret)
        {
            this.Client = new HttpClient();
            JWTTocken = JWTBuilder.Build(Payload, secret);
        }

        public async Task<string> Request(string url)
        {
            this.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTTocken);

            HttpResponseMessage response = await this.Client.GetAsync(url);

            HttpContent content = response.Content;
            
            //returns json string 
            return await content.ReadAsStringAsync();
        }

        private readonly string Payload = "{\"name\":\"John Doe\"}";
        private readonly HttpClient Client;
        private string JWTTocken;

        public string JtwTocken { set => JWTTocken = JWTBuilder.Build(Payload, value); }
    }
}
