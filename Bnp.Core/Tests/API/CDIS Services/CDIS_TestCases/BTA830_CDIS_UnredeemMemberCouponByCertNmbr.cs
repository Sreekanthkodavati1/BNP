using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA830_CDIS_UnredeemMemberCouponByCertNmbr : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;


        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_UnredeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_UnredeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA830_SOAP_UnredeemMemberCouponByCertNmbr()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double time = 0;
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_CertNumber_From_LWMemberCouponTable_With_EmptyDateRedeemed"];
                
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid certificateNumber from LW_MemberCoupon table";
                string validCertNumber = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "CERTIFICATENMBR", sqlstmt);
                testStep.SetOutput("The valid certNumbr from LW_MemberCoupon table is: " + validCertNumber);
                Logger.Info("The valid certNumbr from LW_MemberCoupon table is: " + validCertNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using certNumber using RedeemMemberCouponByCertNumber method";
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                string dbStatus = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "STATUS", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate+
                                   "; and coupon status is:" + dbStatus);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate +
                                   " and coupon status is:" + dbStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Unredeem the above member coupon using certNumber using UnRedeemMemberCouponByCertNumber method";
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out time);
                string dbRedemptionDateAfterUnRedeem = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                string dbStatusAfterUnRedeem = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "STATUS", string.Empty);
                Assert.AreEqual("", dbRedemptionDateAfterUnRedeem, "Expected value is [] Actual value is" + dbRedemptionDateAfterUnRedeem);
                Assert.AreEqual("Active", dbStatusAfterUnRedeem, "Expected value is Active" + " and Actual value is" + dbStatusAfterUnRedeem);
                testStep.SetOutput("After unredeeming the coupon:; the redemption date is: [NULL]" + dbRedemptionDateAfterUnRedeem +
                                   ";coupon status is:" + dbStatusAfterUnRedeem);
                Logger.Info("TestStep: " + stepName + " ##Passed## After unredeeming the coupon:; the redemption date is: " + dbRedemptionDateAfterUnRedeem +
                                   ";coupon status is:" + dbStatusAfterUnRedeem);
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_UnredeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_UnredeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1237_ST1535_SOAP_UnredeemMemberCouponByCertNmbr_WithElapsedTime()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            double time = 0;

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_CertNumber_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid certificateNumber from LW_MemberCoupon table";
                string validCertNumber = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "CERTIFICATENMBR", sqlstmt);
                testStep.SetOutput("The valid certNumbr from LW_MemberCoupon table is: " + validCertNumber);
                Logger.Info("The valid certNumbr from LW_MemberCoupon table is: " + validCertNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using certNumber using RedeemMemberCouponByCertNumber method";
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                string dbStatus = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "STATUS", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate +
                                   "; and coupon status is:" + dbStatus);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate +
                                   " and coupon status is:" + dbStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Unredeem the above member coupon by providing certNumber using UnRedeemMemberCouponByCertNumberWithElapsedTime method";
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out time);
                string dbRedemptionDateAfterUnRedeem = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                string dbStatusAfterUnRedeem = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "STATUS", string.Empty);
                Assert.AreEqual("", dbRedemptionDateAfterUnRedeem, "Expected value is [] Actual value is" + dbRedemptionDateAfterUnRedeem);
                Assert.AreEqual("Active", dbStatusAfterUnRedeem, "Expected value is Active" + " and Actual value is" + dbStatusAfterUnRedeem);
                testStep.SetOutput("After unredeeming the coupon:; the redemption date is: [NULL]" + dbRedemptionDateAfterUnRedeem +
                                   ";coupon status is:" + dbStatusAfterUnRedeem +" and the elapsed time is: " + time);
                Logger.Info("TestStep: " + stepName + " ##Passed## After unredeeming the coupon:; the redemption date is: " + dbRedemptionDateAfterUnRedeem +
                                   ";coupon status is:" + dbStatusAfterUnRedeem);
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_UnredeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_UnredeemMemberCouponByCertNmbr_Negative")]
        [TestMethod]
        public void BTA1237_ST1536_SOAP_UnredeemMemberCouponByCertNmbr_ByPassingInvalidCertNmbr()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {           
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Unredeem the above member coupon by providing invalid certNumber";
                string error = (string)cdis_Service_Method.UnredeemMemberCouponByCertNmbrNegative();
                testStep.SetOutput("Throws an expection with the " + error);
                string[] errors = error.Split(';');
                string[] errorssplit = errors[0].Split('=');
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response for Error Code as 3370";
                Assert.AreEqual("3370", errorssplit[1], "Expected value is" + "3370" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errors[1]);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
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