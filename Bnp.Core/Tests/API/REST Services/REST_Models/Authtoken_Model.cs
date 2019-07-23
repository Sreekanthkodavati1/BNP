using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.REST_Services.REST_Models
{
    public class Authtoken_Model
    {
        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public Authtoken_Model()
        {
            this.clientId = ConfigurationManager.AppSettings["clientId"];
            this.clientSecret = ConfigurationManager.AppSettings["clientSecret"];


        }

        public Authtoken_Model(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }
    }
}



