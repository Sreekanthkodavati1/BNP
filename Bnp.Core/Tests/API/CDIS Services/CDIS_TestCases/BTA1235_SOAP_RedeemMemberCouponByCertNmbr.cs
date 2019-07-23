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

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1235_SOAP_RedeemMemberCouponByCertNmbr : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime;
        int? timesused = 1;
        DateTime redemptiondate;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1235_ST1301_SOAP_RedeemMemberCouponByCertNmbr_ExistingChannel()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon using certNumber with an existing channel";
                string channel = DatabaseUtility.GetFromSoapDB("Lw_Channeldef", string.Empty, string.Empty, "NAME", string.Empty);
                redemptiondate = System.DateTime.Now;
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber, channel, "en", redemptiondate, timesused, false, false, string.Empty, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the elapsed time is greater than Zero";
                if (elapsedTime > 0)
                {
                    testStep.SetOutput("Elapsed time is greater than zero and the Elapsed time is " + elapsedTime);
                }
                else
                {
                    throw new Exception("Elapsed time is not greater than Zero");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1235_ST1302_SOAP_RedeemMemberCouponByCertNmbr_ExistingLanguage()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon using certNumber with an existing language";
                string language = DatabaseUtility.GetFromSoapDB("Lw_Languagedef", string.Empty, string.Empty, "CULTURE", string.Empty);
                redemptiondate = System.DateTime.Now;
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber, "Web", language, redemptiondate, timesused, false, false, string.Empty, out elapsedTime);

                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1235_ST1303_SOAP_RedeemMemberCouponByCertNmbr_CertNumberFromDb()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon using certNumber where certNumber exists in database";
                DateTime redemptiondate = System.DateTime.Now;
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber, "Web", "en", redemptiondate, timesused, false, false, string.Empty, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1235_ST1321_SOAP_RedeemMemberCouponByCertNmbr_ReturnAttributeFalse()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon using certNumber with Return attributes False";
                redemptiondate = System.DateTime.Now;
                bool? returnattributes = false;
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber, "Web", "en", redemptiondate, timesused, returnattributes, false, string.Empty, out elapsedTime);

                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1235_ST1322_SOAP_RedeemMemberCouponByCertNmbr_ReturnAttributeTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon using certNumber with Return attributes true";
                redemptiondate = System.DateTime.Now;
                bool? returnattributes = true;
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber, "Web", "en", redemptiondate, timesused, returnattributes, false, string.Empty, out elapsedTime);

                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1235_ST1323_SOAP_RedeemMemberCouponByCertNmbr_TimeUsed_1()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon using certNumber with Time used as 1";
                redemptiondate = System.DateTime.Now;
                timesused = 1;
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber, "Web", "en", redemptiondate, timesused, false, false, string.Empty, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Positive")]
        [TestMethod]
        public void BTA1235_ST1324_SOAP_RedeemMemberCouponByCertNmbr_RedemptionDateCurrent()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon using certNumber with current redemption date";
                redemptiondate = System.DateTime.Now;
                var certDetails = cdis_Service_Method.RedeemMemberCouponByCertNumber(validCertNumber, "Web", "en", redemptiondate, timesused, false, false, string.Empty, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", validCertNumber, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponByCertNumber method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Negative")]
        [TestMethod]
        public void BTA1235_ST1433_SOAP_RedeemMemberCouponByCertNmbr_NonExistingChannel()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon with non existing channel and validate the Error Code:6003";
                redemptiondate = System.DateTime.Now;
                string channel = "test";
                string error = cdis_Service_Method.RedeemMemberCouponByByCertNumber_Invalid(validCertNumber, channel, "en", redemptiondate, timesused, false, false, string.Empty);
                if (error.Contains("Error code=6003") && error.Contains("Error Message=Specified channel is not defined"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6003. Actual error received is:\"" + error + "\"");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Negative")]
        [TestMethod]
        public void BTA1235_ST1434_SOAP_RedeemMemberCouponByCertNmbr_NonExistingLanguage()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
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
                stepName = "Redeem the member coupon with non existing language and validate the Error Code:6002";
                redemptiondate = System.DateTime.Now;
                string language = "test";
                string error = cdis_Service_Method.RedeemMemberCouponByByCertNumber_Invalid(validCertNumber, "Web", language, redemptiondate, timesused, false, false, string.Empty);
                if (error.Contains("Error code=6002") && error.Contains("Error Message=Specified language is not defined"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6002. Actual error received is:\"" + error + "\"");
                }

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                //unredeem the cert again since we have less certificates
                cdis_Service_Method.UnRedeemMemberCouponByCertNumber(validCertNumber, out elapsedTime);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr")]
        [TestCategory("API_SOAP_RedeemMemberCouponByCertNmbr_Negative")]
        [TestMethod]
        public void BTA1235_ST1435_SOAP_RedeemMemberCouponByCertNmbr_NonExistingCertNmbr()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_CertNumber_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon with non existing Certificate number and validate the Error code:3370";
                redemptiondate = System.DateTime.Now;
                string invalidCertNumber = common.RandomNumber(10) +"-"+ common.RandomNumber(4);
                string error = cdis_Service_Method.RedeemMemberCouponByByCertNumber_Invalid(invalidCertNumber, "Web", "en", redemptiondate, timesused, false, false, string.Empty);
                if (error.Contains(" Error code=3370") && error.Contains("Error Message=No coupon could be located using cert number"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:3370. Actual error received is:\"" + error + "\"");
                }

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
