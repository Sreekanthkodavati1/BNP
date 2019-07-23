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
    public class BTA225_CDIS_EnrollMemberPromotion : ProjectTestBase
    {

        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA225_CDIS_EnrollMemberPromotion_Positive()
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
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Promotion Definitions from Service";
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
                testStep.SetOutput("The Targerted Promotion: " + promot.Name +" will be added to the member; Name: " + output.FirstName);
                Logger.Info("Promotion to be added " + promot.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding targeted Promotion to a member";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, promot.Code);
                testStep.SetOutput("The Targerted Promotion: " + promot.Name+ " with Promotion ID : " + promotionCode.Id+" has been added to member: "+ output.FirstName+" and Enroll flag is set to : "+promotionCode.Enrolled);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Enroll Promotion member";
                MemberPromotionStruct Enrollpromotionmember = cdis_Service_Method.EnrolledPromotionMember(vc[0].LoyaltyIdNumber, promot.Code);
                testStep.SetOutput("Member has been enrolled for Promotion and enroll flag status is : " + Enrollpromotionmember.Enrolled);
                Logger.Info("Enroll MemberPromotion : " + Enrollpromotionmember.Enrolled);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetEnrollMemberPromotionsFromDBSOAP(output.IpCode + "");            
                Assert.AreEqual(Enrollpromotionmember.Enrolled + "", (EnrollPromotion_Status)Convert.ToInt32(dbresponse) + "", "Expected value is" + Enrollpromotionmember.Enrolled + "Actual value is" + (EnrollPromotion_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("Response from database for ENROLLED column in LW_MemberPromotion table is " + dbresponse + " which means member enroll status is set to true");
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

