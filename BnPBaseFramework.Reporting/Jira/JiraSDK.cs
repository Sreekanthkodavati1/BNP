using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Jira;

namespace BnPBaseFramework.Reporting.Jira
{
    public class JiraSDK
    {
        public static string jiraURL = "https://brierley.atlassian.net";
        public static string userName = ConfigurationManager.AppSettings["JiraUser"];
        public static string password = ConfigurationManager.AppSettings["JiraPassword"];

        public Atlassian.Jira.Jira AuthenticateJira()
        {
            var atlassian = Atlassian.Jira.Jira.CreateRestClient(jiraURL, userName, password);
            return atlassian;
        }

        public string CreateTestPlan()
        {
            var issue = AuthenticateJira().CreateIssue("BTA");
            issue.Type = "Test Plan";
            issue.Summary = "Test Plan on " + DateTime.UtcNow.ToString("MMddyyyy");
            issue.Description = "Test Plan for Test Automation Run on " + DateTime.UtcNow.ToString("MMddyyyy");
            issue.SaveChanges();
            var testPlanKey = issue.Key;
            return testPlanKey.ToString();
        }
                       
    }
}
