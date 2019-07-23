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
    public class BTA239_CDIS_RedemMemberCouponById : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA239_CDIS_RedemMemberCouponById_Positive()
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
                stepName = "Getting Coupon Definitions from GetCouponDefinitions method";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                testStep.SetOutput("First Coupon Name : " + def.CouponDefinition[0].Name);
                Logger.Info("First Coupon Name : " + def.CouponDefinition[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member using AddMemberCoupon";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, def.CouponDefinition[0].Id);
                testStep.SetOutput("Coupon is added to member and the MemberCouponId is : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Number of times Coupons used before redeeming using GetMemberCoupons method";
                MemberCouponStruct[] memberCoupons = cdis_Service_Method.GetMemberCoupons(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Number of times Coupon Used is : " + memberCoupons[0].TimesUsed);
                Logger.Info("Number of times Coupon Used is  : " + memberCoupons[0].TimesUsed);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon of by ID using RedeemMemberCouponById";
                RedeemMemberCouponByIdOut redeemMemberCoupons = cdis_Service_Method.RedeemMemberCouponById(memberCouponId);
                testStep.SetOutput("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " and the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                Logger.Info("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " And the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Number of times Coupons used before redeeming using GetMemberCoupons methode";
                MemberCouponStruct[] memberCouponsnew = cdis_Service_Method.GetMemberCoupons(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(memberCoupons[0].TimesUsed + 1, memberCouponsnew[0].TimesUsed, "Expected Value is :" + (memberCoupons[0].TimesUsed + 1), " Actual Value is : " + memberCouponsnew[0].TimesUsed);
                testStep.SetOutput("TimesUsed: Expected value is :" + (memberCoupons[0].TimesUsed + 1) + " and Actual Value is : " + memberCouponsnew[0].TimesUsed);
                Logger.Info("Number of times Coupon Used is  : " + memberCouponsnew[0].TimesUsed);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the redemptiondate from GetMemberCoupons Method";
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), memberCouponsnew[0].DateRedeemed.Value.ToString("MM/dd/yyyy"), "Expected Value is :" + System.DateTime.Now.ToString("MM/dd/yyyy"), " Actual Value is : " + memberCouponsnew[0].DateRedeemed.Value.ToString("MM/dd/yyyy"));
                testStep.SetOutput("Redemption Date: Expected value is :" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual Value is : " + memberCouponsnew[0].DateRedeemed.Value.ToString("MM/dd/yyyy"));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate redemptiondate in LW_MEMBERCOUPON table and redemption record has been created in LW_MemberCouponRedemption";
                string dbresponse = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", memberCouponId.ToString(), "DATEREDEEMED", string.Empty);
                string dbresponse1 = DatabaseUtility.GetFromSoapDB("LW_MemberCouponRedemption", "MemberCouponID", memberCouponId.ToString(), "DATEREDEEMED", string.Empty);
                Assert.AreEqual(memberCouponsnew[0].DateRedeemed.ToString(), dbresponse, "Expected Value is :" + memberCouponsnew[0].DateRedeemed.ToString(), " Actual Value is : " + dbresponse);
                testStep.SetOutput("Redemption Date: from the response of GetMemberCoupons :" + memberCouponsnew[0].DateRedeemed + " and from DB (LW_MEMBERCOUPON) : " + dbresponse+
                                    ";Verified the record in LW_MemberCouponRedemption and captured redemption date : "+ dbresponse1);
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

    

   