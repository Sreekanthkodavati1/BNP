using System;
using System.Globalization;

namespace Bnp.Core.WebPages.Models
{
    public class Promotions
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Enrollmenttype { get; set; }
        public string AttributeName { get; set; }
        public string ValueToSetInAttribute { get; set; }
        
    }
}
