using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BnPBaseFramework.Reporting.Utils
{
    public class StringHelper
    {
        public string GetFormattedDateTimeNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));            
        }
        public string GetFormattedDateTimeNow(string jira)
        {
            return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");           
        }
    }
}
