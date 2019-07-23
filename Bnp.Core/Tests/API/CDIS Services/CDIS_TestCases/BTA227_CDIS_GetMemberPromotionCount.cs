using System;
using System.Collections.Generic;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{

    [TestClass]

    public class BTA227_CDIS_GetMemberPromotionCount : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA227_CDIS_GetMemberPromotionCount_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            int index = 0;

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " ,Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Promotion Definitions from Service";
                if (cdis_Service_Method.GetActivePromotionsDefinitionsCount() < 15)
                {
                    index = 0;
                }
                else
                {
                    index = cdis_Service_Method.GetActivePromotionsDefinitionsCount() - 10;
                }

                PromotionDefinitionStruct[] def = cdis_Service_Method.GetPromotionDefinitionsRecent(index);
                PromotionDefinitionStruct promot = new PromotionDefinitionStruct();
                foreach (PromotionDefinitionStruct pd in def)
                {

                    if (pd.Targeted)
                    {
                        promot = pd;
                        break;
                    }
                }
                testStep.SetOutput("The Targerted Promotion: " + promot.Name + " will be added to the member; Name: " + output.FirstName);
                Logger.Info("Promotion to be added " + promot.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a member from Service";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, promot.Code);
                testStep.SetOutput("The Targerted Promotion: " + promot.Name + " with Promotion ID : " + promotionCode.Id + " has been added to member: " + output.FirstName);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get PromotionCount for a Member from Service";
                int PromotionMemberCount = cdis_Service_Method.GetPromotionMembersCount(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The MemberPromotion Count for the above member from the response of GetPromotionMemberCount method: " + PromotionMemberCount);
                Logger.Info("MemberPromotion Count : " + PromotionMemberCount);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the Member promotion count from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCountUsingIdFromDBSOAP(output.IpCode + "");
                Assert.AreEqual(PromotionMemberCount + "", dbresponse, "Expected value is" + PromotionMemberCount + "Actual value is" + dbresponse);
                testStep.SetOutput("Member Promotion Count from database:" + dbresponse);
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
