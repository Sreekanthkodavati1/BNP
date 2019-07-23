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
    public class BTA236_CDIS_GetMemberMessages : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA236_CDIS_GetMemberMessages_PositiveCase()

        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Issue Message using Trigger event method";
                TriggerUserEventOut userEventOut = cdis_Service_Method.UserTriggerEvent(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member Message Id:" + userEventOut.MemberMessage[0].Id + " and Member MessageDefId:" + userEventOut.MemberMessage[0].MessageDefId + " from " +
                    " the response of TrigggerEvent method");
                Logger.Info("Member MessageID: " + userEventOut.MemberMessage[0].Id + "Member MessageDefID: " + userEventOut.MemberMessage[0].MessageDefId+ " from " +
                    " the response of TrigggerEvent method");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get member messageID and MessagedDefID from GetMemberMessages method";
                GetMemberMessagesOut memberMessagesOut = cdis_Service_Method.GetMemberMessages(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member MessageID:" + memberMessagesOut.MemberMessage[0].Id + " and Member MessageDefID:" + memberMessagesOut.MemberMessage[0].MessageDefId+" from the GetMemberMessages response");
                Assert.AreEqual(userEventOut.MemberMessage[0].Id, memberMessagesOut.MemberMessage[0].Id, "Expected Value is :" + userEventOut.MemberMessage[0].Id, "Actual  Value is :" + memberMessagesOut.MemberMessage[0].Id);
                Assert.AreEqual(userEventOut.MemberMessage[0].MessageDefId, memberMessagesOut.MemberMessage[0].MessageDefId, "Expected Value is :" + userEventOut.MemberMessage[0].MessageDefId, "Actual  Value is :" + memberMessagesOut.MemberMessage[0].MessageDefId);
                Logger.Info("Member MessageID: " + memberMessagesOut.MemberMessage[0].Id + " and Member MessageDefID: " + memberMessagesOut.MemberMessage[0].MessageDefId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating MessageDefID of the user with data from DB";
                string dbresponse = DatabaseUtility.GetMemberMessageDefIDfromDBUsingIdSOAP(memberMessagesOut.MemberMessage[0].Id + "");
                testStep.SetOutput("Message Def ID feteched from DB is : " + dbresponse);
                Assert.AreEqual(memberMessagesOut.MemberMessage[0].MessageDefId + "", dbresponse, "Expected Value is " + memberMessagesOut.MemberMessage[0].MessageDefId + " Actual Value is " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
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

