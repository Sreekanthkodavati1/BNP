using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BnPBaseFramework.Reporting.Base
{
    public class TestStep : TestCase
    {
        private String testStep;

        private bool status;

        private String input;

        private String TestCaseid;

        private String output;

        private String expectedResult;

        private String stepStartTime;

        private String stepJiraStartTime;

        private String stepEndTime;

        private String stepJiraEndTime;

        private String errorMessage;

        private String imageContent;

        private int TestStepdbstatus;
        private List<String> images;


        public int GetTestStepdbstatus()
        {
            return this.TestStepdbstatus;
        }

        public void SetTestStepdbstatus(int dbstatus)
        {
            this.TestStepdbstatus = dbstatus;
        }


        public String GetTestStep()
        {
            return this.testStep;
        }

        public void SetTestStep(String testStep)
        {
            this.testStep = testStep;
        }

        public bool IsStatus()
        {
            return this.status;
        }

        public void SetStatus(bool status)
        {
            this.status = status;
        }


        public String GetTestCaseid()
        {
            return this.TestCaseid;
        }

        public void SetTestCaseid(String TestCaseid)
        {
            this.TestCaseid = TestCaseid;
        }

        public String GetInput()
        {
            return this.input;
        }

        public void SetInput(String input)
        {
            this.input = input;
        }

        public String GetOutput()
        {
            return this.output;
        }

        public void SetOutput(String output)
        {
            this.output = output;
        }

        public String GetExpectedResult()
        {
            return this.expectedResult;
        }

        public void SetExpectedResult(String expectedResult)
        {
            this.expectedResult = expectedResult;
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

        public List<String> GetImages()
        {
            return this.images;
        }

        public void SetImages(List<String> images)
        {
            this.images = images;
        }

        public String GetStepStartTime()
        {
            return this.stepStartTime;
        }

        public String GetJiraStepStartTime()
        {
            return this.stepJiraStartTime;
        }

        public void SetStepStartTime(String stepStartTime)
        {
            this.stepStartTime = stepStartTime;
        }
        public void SetJiraStepStartTime(String stepJiraStartTime)
        {
            this.stepJiraStartTime = stepJiraStartTime;
        }

        public String GetStepEndTime()
        {
            return this.stepEndTime;
        }

        public String GetJiraStepEndTime()
        {
            return this.stepJiraEndTime;
        }

        public void SetStepEndTime(String stepEndTime)
        {
            this.stepEndTime = stepEndTime;
        }
        public void SetJiraStepEndTime(String stepJiraEndTime)
        {
            this.stepJiraEndTime = stepJiraEndTime;
        }
    }
}



