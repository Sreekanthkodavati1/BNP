using BnPBaseFramework.Reporting.Base;
using System;
using System.Globalization;


namespace BnPBaseFramework.Reporting.Utils
{
   public class TestStepHelper
    {
        public static TestStep StartTestStep(TestStep testStep)
        {
            testStep = new TestStep();
            testStep.SetStepStartTime(new StringHelper().GetFormattedDateTimeNow());
            testStep.SetJiraStartTime(new StringHelper().GetFormattedDateTimeNow("jira"));
            return testStep;
        }

        //public static TestStep EndTestStep(TestCase testCase,TestStep testStep, String testStepName, bool status, DriverContext driverContext)
        //{
        //    testStep.SetStepEndTime(new StringHelper().GetFormattedDateTimeNow());
        //    testStep.SetTestStep(testStepName);
        //    testStep.SetStatus(status);   
        //    if (!testCase.GetTestCaseName().StartsWith("API_"))
        //    {
        //        testStep.SetImageContent(driverContext.TakeScreenshot().ToString());
        //    }
                        
        //    return testStep;
        //}

        public static TestStep EndTestStep(TestCase testCase, TestStep testStep, String testStepName, bool status, string imageContent)
        {
            testStep.SetStepEndTime(new StringHelper().GetFormattedDateTimeNow());
            testStep.SetJiraStepEndTime(new StringHelper().GetFormattedDateTimeNow("jira"));
            testStep.SetTestStep(testStepName);
            testStep.SetStatus(status);
            if (!string.IsNullOrEmpty(imageContent))
            {
                //testStep.SetImageContent(driverContext.TakeScreenshot().ToString());
                testStep.SetImageContent(imageContent);
            }

            return testStep;
        }
    }
}
