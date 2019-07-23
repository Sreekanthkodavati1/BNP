using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BnpBaseFramework.API.Utils
{
    public class BaseClass
    {
        public String Endpoint { get; set; }
        public Dictionary<String, String> parameters { get; set; }
        public Dictionary<String, String> headers { get; set; }
        public String body { get; set; }

        public BaseClass()
        {
            this.Endpoint = "";
            this.parameters = null;
            this.headers = null;
            this.body = null;
        }

        public BaseClass(String Endpoint, Dictionary<String, String> parameters, Dictionary<String, String> headers,String body)
        {
            this.Endpoint = Endpoint;
            this.parameters = parameters;
            this.headers = headers;
            this.body = body;
        }
    }
}
