using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BnPBaseFramework.Reporting.Jira
{
    public class JiraModel
    {
        public class JiraTestSuite
        {
            public JiraInfo info { get; set; }
            public List<JiraTest> tests { get; set; }            
        }
        public class JiraInfo
        {
            public string summary { get; set; }
            public string description { get; set; }
            public string user { get; set; }
            public string startDate { get; set; }
            public string finishDate { get; set; }
            public string testPlanKey { get; set; }
        }
        public class JiraTest
        {
            public string testKey { get; set; }
            public string start { get; set; }
            public string finish { get; set; }
            public string comment { get; set; }
            public string status { get; set; }
            public List<JiraStep> steps { get; set; }
        }
        public class JiraStep
        {
            public string status { get; set; }
            public string comment { get; set; }                        
        }
        public class JiraAuth
        {
            public string client_id { get; set; }
            public string client_secret { get; set; }
        }
    }

}
