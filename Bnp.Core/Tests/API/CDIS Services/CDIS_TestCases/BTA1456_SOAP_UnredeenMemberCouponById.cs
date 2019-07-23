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
    public class BTA1456_SOAP_UnredeenMemberCouponById:ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_UnredeenMemberCouponById")]
        [TestCategory("API_SOAP_UnredeenMemberCouponById_Positive")]
        [TestMethod]
        public void BTA1456_1730_SOAP_Regression_UnredeenMemberCouponById_PassingMandatoryValues()
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
                stepName = "Get the time used from LW_MEMBERCOUPON table before redemption";
                string dbresponse = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", memberCouponId.ToString(), "TIMESUSED", string.Empty);
                testStep.SetOutput("Times used for the Member Coupons before Redemption: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon of by ID using RedeemMemberCouponById";
                RedeemMemberCouponByIdOut redeemMemberCoupons = cdis_Service_Method.RedeemMemberCouponById(memberCouponId);
                string timesusedAfterRedemption = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", memberCouponId.ToString(), "TIMESUSED", string.Empty);
                testStep.SetOutput("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + "; Number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft+ "; And Times Used from DB is"+ timesusedAfterRedemption);
                Logger.Info("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " And the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "UnRedeeming the Coupon of by ID using UnRedeemMemberCouponById and verify times used in DB";
                cdis_Service_Method.UnRedeemMemberCouponById(memberCouponId);
                string dbresponse2 = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", memberCouponId.ToString(), "TIMESUSED", string.Empty);
                Assert.AreEqual(dbresponse, dbresponse2, "Expected Value is :" + dbresponse, " Actual Value is : " + dbresponse2);
                testStep.SetOutput("Times used for the Member Coupons before Redemption: "+ dbresponse + " and after unredeeming the coupon: " + dbresponse2);
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_UnredeenMemberCouponById")]
        [TestCategory("API_SOAP_UnredeenMemberCouponById_Negative")]
        [TestMethod]
        public void BTA1456_1731_SOAP_Regression_UnredeenMemberCouponById_InvalidCouponID()
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
                stepName = "UnRedeeming the Coupon of by ID using RedeemMemberCouponById";
                long memberCouponIdinvalid = Convert.ToInt32(common.RandomNumber(5));
                string response = (string)cdis_Service_Method.UnRedeemMemberCouponByIdNegative(memberCouponIdinvalid);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');
                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("3370", errorssplit[1], "Expected value is" + "3370" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_UnredeenMemberCouponById")]
        [TestCategory("API_SOAP_UnredeenMemberCouponById_Positive")]
        [TestMethod]
        public void BTA1456_1732_SOAP_Regression_UnredeenMemberCouponById_CouponNeverRedeemed()
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
                stepName = "Get the time used from LW_MEMBERCOUPON table before redemption";
                string dbresponse = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", memberCouponId.ToString(), "TIMESUSED", string.Empty);
                Assert.AreEqual("0", dbresponse, "Expected Value is : 0, and Actual Value is : " + dbresponse);
                testStep.SetOutput("Times used for the Member Coupons before Redemption: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon of by ID using RedeemMemberCouponById";
                RedeemMemberCouponByIdOut redeemMemberCoupons = cdis_Service_Method.RedeemMemberCouponById(memberCouponId);
                string timesusedAfterRedemption = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", memberCouponId.ToString(), "TIMESUSED", string.Empty);
                testStep.SetOutput("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + "; Number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft + "; And Times Used from DB is" + timesusedAfterRedemption);
                Logger.Info("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " And the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "UnRedeeming the Coupon of by ID using UnRedeemMemberCouponById and verify times used in DB";
                cdis_Service_Method.UnRedeemMemberCouponById(memberCouponId);
                string dbresponse2 = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", memberCouponId.ToString(), "TIMESUSED", string.Empty);
                Assert.AreEqual(dbresponse, dbresponse2, "Expected Value is :" + dbresponse, " Actual Value is : " + dbresponse2);
                testStep.SetOutput("Times used for the Member Coupons before Redemption: " + dbresponse + " and after unredeeming the coupon: " + dbresponse2);
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
