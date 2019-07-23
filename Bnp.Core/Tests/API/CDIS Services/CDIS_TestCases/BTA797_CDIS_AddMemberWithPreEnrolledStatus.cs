using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
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
    public class BTA797_CDIS_AddMemberWithPreEnrolledStatus : ProjectTestBase
    {

        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA797_CDIS_AddMember_PreEnrolled_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            String stepName = "";

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberPreEnrolled();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName+ " and the member status is: "+output.MemberStatus); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Memberstatus in DB";
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                //string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "NewStatus", string.Empty);
                Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
                testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: "+ output.MemberStatus.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
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
                testCase.SetErrorMessage(e.Message);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
                Assert.Fail(e.Message.ToString());
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
