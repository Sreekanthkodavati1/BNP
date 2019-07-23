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
    public class BTA248_CDIS_IsMemberCouponRedeemable : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA248_CDIS_IsMemberCouponRedeemable_Positive()
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
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get the Coupon Definitions from GetCouponDefinitions";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                testStep.SetOutput("First Coupon Name : " + def.CouponDefinition[0].Name);
                Logger.Info("First Coupon Name : " + def.CouponDefinition[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding a Coupon to a member from AddMemberCoupon service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, def.CouponDefinition[0].Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Is Member Coupon Redemable";
                IsMemberCouponRedeemableOut memberCouponRedeemableOut = cdis_Service_Method.IsMemberCouponRedeemable(memberCouponId);
                testStep.SetOutput("Is MemberCoupon Redeemable : " + memberCouponRedeemableOut.Redeemable);
                Assert.AreEqual(true, memberCouponRedeemableOut.Redeemable, "Expected value is" + true + "Actual value is" + memberCouponRedeemableOut.Redeemable);
                Logger.Info(" Is MemberCoupon Redeemable : " + memberCouponRedeemableOut.Redeemable);
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