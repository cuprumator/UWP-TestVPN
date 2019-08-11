using System.Threading.Tasks;

namespace TestVPN.RESTClient
{
    interface IRESTClient
    {
        Task<string> Request(string url);
    }
}
