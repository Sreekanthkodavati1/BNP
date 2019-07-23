using BnpBaseFramework.API.Utils;
using BnPBaseFramework.Reporting.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BnPBaseFramework.Reporting.Jira
{
    public class XRay : JiraModel
    {
       public JiraTest GenerateTestCase(TestCase tc)
        {
            JiraTest jiraTc = new JiraTest();
            //jiraTc.testKey = tc.GetTestCaseName().Split('_')[0];
            jiraTc.testKey = "BTA-1606";
            jiraTc.start = tc.GetJiraStartTime();
            jiraTc.finish = tc.GetJiraEndTime();
            jiraTc.comment = tc.GetTestCaseName();
            if (tc.IsStatus())
                jiraTc.status = "PASSED";
            else
                jiraTc.status = "FAILED";
            jiraTc.steps = new List<JiraStep>();
            foreach (TestStep step in tc.GetTestCaseSteps())
            {
                jiraTc.steps.Add(GenerateTestStep(step));
                
            }
            return jiraTc;
        }
        public JiraInfo GenerateInfo(TestSuite ts, TestSuite tjs)
        {
            JiraInfo jiraInfo = new JiraInfo();
            JiraSDK jiraSDK = new JiraSDK();
            jiraInfo.summary = "Execution of test suite " + ts.GetSuiteName();
            jiraInfo.description = "This is automated Jira Integration Run";
            jiraInfo.user = "cchavva";
            jiraInfo.startDate = tjs.GetSuiteStartTime();
            jiraInfo.finishDate = tjs.GetSuiteEndTime();
            jiraInfo.testPlanKey = jiraSDK.CreateTestPlan();
            return jiraInfo;
        }
        public JiraStep GenerateTestStep(TestStep ts)
        {
            JiraStep jiraStep = new JiraStep();
             if (ts.IsStatus())
                jiraStep.status = "PASSED";
            else
                jiraStep.status = "FAILED";
            jiraStep.comment = ts.GetTestStep();
            return jiraStep;
        }
        public JiraTestSuite GenerateTestSuite(TestSuite ts, TestSuite tjs)
        {
            JiraTestSuite jiraSuite = new JiraTestSuite();
            jiraSuite.info = GenerateInfo(ts, tjs);
            jiraSuite.tests = new List<JiraTest>();
            foreach (TestCase tc in ts.GetListOfTestCases())
            {
                jiraSuite.tests.Add(GenerateTestCase(tc));
            }

            return jiraSuite;
        }
        public JiraAuth GenerateAuth()
        {
            JiraAuth jiraAuth = new JiraAuth {
                client_id = ConfigurationManager.AppSettings["JiraClientID"],
                client_secret = ConfigurationManager.AppSettings["JiraClientSecret"]
        };

            return jiraAuth;
        }
        public string ConvertToJsonBody(object obj)
        {
            String jstring;
            try
            {
                jstring = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                return jstring;
            }
            catch
            {
                throw new Exception("unable to convert in  json string");
            }
        }
        public String ResponseToString(HttpWebResponse response)
        {
            String responseString = "";
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseString = reader.ReadToEnd();
                    }
            }
            return responseString;
        }
        public string[] PostAuthToken()
        {
            BaseClass auth = new BaseClass();
            auth.body = ConvertToJsonBody(GenerateAuth());
            auth.Endpoint = "authenticate";
            var response_req = (HttpWebResponse)new BaseLibrary().PostJiraRestRequest(auth);
            return response_req.Headers.GetValues("x-access-token");
        }
        public String GetAuthToken()
        {
            string[] auth = PostAuthToken();
            string tokenvalue = auth[0];

            return tokenvalue;
        }
        public Object PostJiraResults(TestSuite ts, TestSuite tjs)
        {
            BaseClass structure = new BaseClass();
            structure.body = ConvertToJsonBody(GenerateTestSuite(ts, tjs));
            structure.Endpoint = "import/execution";
            Dictionary<String, String> header = new Dictionary<string, string>();
            
            header.Add("Authorization", "Bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostJiraRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(ResponseToString(response_req));
        }
    }
}
