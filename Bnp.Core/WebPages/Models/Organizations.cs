using BnPBaseFramework.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.WebPages.Models
{
    public class Organizations : ProjectBasePage
    {
        public Organizations(DriverContext driverContext)
          : base(driverContext)
        {
        }
        public string organization { get; set; }

        

    }
}
