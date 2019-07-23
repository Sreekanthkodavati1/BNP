using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA798_CDIS_GetRewardCategories : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA798_CDIS_GetRewardCategories_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get the ParentCategoryID from LW_Category table";
                string dbresponse = DatabaseUtility.GetFromSoapDB("LW_Category", string.Empty, null, "PARENTCATEGORYID", string.Empty);
                testStep.SetOutput("The ParentCategoryID from LW_Category table is: " + dbresponse);
                Logger.Info(stepName+ " : Passed: The ParentCategoryID from LW_Category table is: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the CategoryID's in LW_Category table";
                var rewardCategories = cdis_Service_Method.GetRewardCategories(long.Parse(dbresponse));
                foreach (var reward in rewardCategories)
                {
                    CategoryID.Add(reward.CategoryId.ToString());
                    string idDB = DatabaseUtility.GetFromSoapDB("LW_Category", "Name", reward.CategoryName.ToString(), "ID", string.Empty);
                    DBCategoryID.Add(idDB);
                    Assert.AreEqual(idDB, reward.CategoryId.ToString(), "Expected value is" + idDB + "Actual value is" + reward.CategoryId.ToString());
                }
                testStep.SetOutput("The CategoryID's from GetRewardCategories response are:; "+
                       string.Join(";", CategoryID.ToArray())+ "; and the categoryID's from DB are:; "+
                        string.Join(";", DBCategoryID.ToArray()));
                Logger.Info(stepName + " : Passed: The CategoryID's from GetRewardCategories response are:## " +
                       string.Join("##", CategoryID.ToArray()) + "; and the categoryID's from DB are:; " +
                        string.Join("##", DBCategoryID.ToArray()));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                Logger.Info("###Test Execution Ends### Test Passed: " + testCase.GetTestCaseName());
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }
    }
}