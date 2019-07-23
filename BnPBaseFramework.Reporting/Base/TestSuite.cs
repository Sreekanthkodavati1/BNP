
using System;
using System.Collections.Generic;


namespace BnPBaseFramework.Reporting.Base
{
    public class TestSuite
    {
        private string suiteName;

        private string suiteStartTime;

        private string suiteJiraStartTime;

        private string suiteEndTime;

        private string suiteJiraEndTime;

        private string environment;

        private string SUT_Host;

        private int totalTestCasesExecuted;

        private int passed;

        private int failed;

        private List<TestCase> listOfTestCases;
        private List<TestStep> listOfTestSteps;

        private string jenkinsBuildURL;

        private string id;

        public TestSuite()
        {
            this.SetId(Guid.NewGuid().ToString());
        }

        public string GetSuiteName()
        {
            return this.suiteName;
        }

        public void SetSuiteName(string suiteName)
        {
            this.suiteName = suiteName;
        }

        public string GetEnvironment()
        {
            return this.environment;
        }

        public void SetEnvironment(String environment)
        {
            this.environment = environment;
        }


        public string GetSUT_Host()
        {
            return this.SUT_Host;
        }

        public void SetSUT_Host(string sUT_Host)
        {
            this.SUT_Host = sUT_Host;
        }

        public int GetPassed()
        {
            return this.passed;
        }

        public void SetPassed(int passed)
        {
            this.passed = passed;
        }

        public int GetFailed()
        {
            return this.failed;
        }

        public void SetFailed(int failed)
        {
            this.failed = failed;
        }

        public List<TestCase> GetListOfTestCases()
        {
            return this.listOfTestCases;
        }

        public void SetListOfTestCases(List<TestCase> listOfTestCases)
        {
            this.listOfTestCases = listOfTestCases;
        }

        public List<TestStep> GetListOfTestSteps()
        {
            return this.listOfTestSteps;
        }

        public void SetListOfTestSteps(List<TestStep> ListOfTestSteps)
        {
            this.listOfTestSteps = ListOfTestSteps;
        }

        public int GetTotalTestCasesExecuted()
        {
            return this.totalTestCasesExecuted;
        }

        public void SetTotalTestCasesExecuted(int totalTestCasesExecuted)
        {
            this.totalTestCasesExecuted = totalTestCasesExecuted;
        }

        public string GetJenkinsBuildURL()
        {
            return this.jenkinsBuildURL;
        }

        public void SetJenkinsBuildURL(string jenkinsBuildURL)
        {
            this.jenkinsBuildURL = jenkinsBuildURL;
        }

        public string GetSuiteStartTime()
        {
            return this.suiteStartTime;
        }

        public string GetJiraSuiteStartTime()
        {
            return this.suiteJiraStartTime;
        }

        public void SetSuiteStartTime(string suiteStartTime)
        {
            this.suiteStartTime = suiteStartTime;
        }

        public void SetJiraSuiteStartTime(string suiteJiraStartTime)
        {
            this.suiteJiraStartTime = suiteJiraStartTime;
        }

        public string GetSuiteEndTime()
        {
            return this.suiteEndTime;
        }

        public string GetJiraSuiteEndTime()
        {
            return this.suiteJiraEndTime;
        }

        public void SetSuiteEndTime(string suiteEndTime)
        {
            this.suiteEndTime = suiteEndTime;
        }

        public void SetJiraSuiteEndTime(string suiteJiraEndTime)
        {
            this.suiteJiraEndTime = suiteJiraEndTime;
        }

        public string GetId()
        {
            return this.id;
        }

        public void SetId(string id)
        {
            this.id = id;
        }

        public string ApplicationName { get; set; }
        public string BrowserName { get; set; }
        public string SuiteName { get; set; }
        public string ClientID { get; set; }
        public string environmentName { get; set; }
        public string buildVersion { get; set; }
    }
}


