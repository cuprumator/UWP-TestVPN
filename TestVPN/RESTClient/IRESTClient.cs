using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVPN.RESTClient
{
    interface IRESTClient
    {
        Task<string> Request(string url);
    }
}
