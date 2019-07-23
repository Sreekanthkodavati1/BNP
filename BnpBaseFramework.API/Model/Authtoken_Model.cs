using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BnpBaseFramework.API.Model
{
	public class Authtoken_Model
	{
		public string clientId { get; set; }

		public string clientSecret { get; set; }

		public Authtoken_Model()
		{
			this.clientId = ConfigurationSettings.AppSettings["clientId"];
			this.clientSecret = ConfigurationSettings.AppSettings["clientSecret"];


		}
	}
}
