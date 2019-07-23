using BnPBaseFramework.Reporting.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BnPBaseFramework.Reporting.Base
{
    public class TestCase
    {
        private String startTime;

        private String startJiraTime;

        private String endTime;

        private String endJiraTime;

        private String testCaseName;

        private bool status;
        private int dbstatus;
        private List<TestStep> testCaseSteps;

        private String errorMessage;
        private String imageContent;

        private List<String> images;

        private String id;

        private String testSuiteId;




        public TestCase()
        {
            this.SetId(Guid.NewGuid().ToString());
        }

        public TestCase(String testCaseName)
        {
            this.SetId(Guid.NewGuid().ToString());
            this.SetStartTime(new StringHelper().GetFormattedDateTimeNow());
            this.SetJiraStartTime(new StringHelper().GetFormattedDateTimeNow("jira"));
            this.SetTestCaseName(testCaseName);
        }

        public List<String> GetImages()
        {
            return this.images;
        }

        public void SetImages(List<String> images)
        {
            this.images = images;
        }

        public int Getdbstatus()
        {
            return this.dbstatus;
        }

        public void Setdbstatus(int dbstatus)
        {
            this.dbstatus = dbstatus;
        }


        public String GetStartTime()
        {
            return this.startTime;
        }

        public String GetJiraStartTime()
        {
            return this.startJiraTime;
        }

        public void SetStartTime(String startTime)
        {
            this.startTime = startTime;
        }
        public void SetJiraStartTime(String startJiraTime)
        {
            this.startJiraTime = startJiraTime;
        }
        public String GetEndTime()
        {
            return this.endTime;
        }
        public String GetJiraEndTime()
        {
            return this.endJiraTime;
        }

        public void SetEndTime(String endTime)
        {
            this.endTime = endTime;
            this.SetJiraEndTime(new StringHelper().GetFormattedDateTimeNow("jira"));
        }
        public void SetJiraEndTime(String endJiraTime)
        {
            this.endJiraTime = endJiraTime;
        }

        public String GetTestCaseName()
        {
            return this.testCaseName;
        }

        public void SetTestCaseName(String testCaseName)
        {
            this.testCaseName = testCaseName;
        }

        public List<TestStep> GetTestCaseSteps()
        {
            return this.testCaseSteps;
        }

        public void SetTestCaseSteps(List<TestStep> testCaseSteps)
        {
            this.testCaseSteps = testCaseSteps;
        }

        public bool IsStatus()
        {
            return this.status;
        }

        public void SetStatus(bool status)
        {
            this.status = status;
        }

        public String GetErrorMessage()
        {
            return this.errorMessage;
        }

        public void SetErrorMessage(String errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public String GetImageContent()
        {
            return this.imageContent;
        }

        public void SetImageContent(String imageContent)
        {
            this.imageContent = imageContent;
        }

        public String GetId()
        {
            return this.id;
        }

        public void SetId(String id)
        {
            this.id = id;
        }


        public string GetTestSuiteId()
        {
            return this.testSuiteId;
        }
        public void SetTestSuiteId(String testSuiteId)
        {
            this.testSuiteId = testSuiteId;
        }
    }
}




