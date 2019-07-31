using System.Net.Http;
using System.Threading.Tasks;
using TestVPN.Utils;

namespace TestVPN.RESTClient
{
    class JWTRESTClient : IRESTClient
    {
        public JWTRESTClient(string secret)
        {
            this.client = new HttpClient();
            jtwTocken = JWTBuilder.Build(payload, secret);
        }

        public async Task<string> Request(string url)
        {
            this.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jtwTocken);

            HttpResponseMessage response = await this.client.GetAsync(url);

            HttpContent content = response.Content;
            
            //returns json string 
            return await content.ReadAsStringAsync();
        }

        private readonly string payload = "{\"name\":\"John Doe\"}";
        private readonly HttpClient client;
        private string jtwTocken;

        public string JtwTocken { set => jtwTocken = JWTBuilder.Build(payload, value); }
    }
}
