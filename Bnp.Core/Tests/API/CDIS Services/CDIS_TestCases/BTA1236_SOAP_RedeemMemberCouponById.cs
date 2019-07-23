using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1236_SOAP_RedeemMemberCouponById : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime;
        int? timesUsed = 1;
        DateTime redemptiondate;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Positive")]
        [TestMethod]
        public void BTA1236_ST1413_SOAP_RedeemMemberCouponById_ExistingChannel()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                var reqCoupon = cdis_Service_Method.getCoupon();
                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
                Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using Id with an existing channel";
                string channel = DatabaseUtility.GetFromSoapDB("Lw_Channeldef", string.Empty, string.Empty, "NAME", string.Empty);
                redemptiondate = System.DateTime.Now;
                var certDetails = cdis_Service_Method.RedeemMemberCouponById(long.Parse(id), channel, "en", redemptiondate, timesUsed, false, false, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", id, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Positive")]
        [TestMethod]
        public void BTA1236_ST1414_SOAP_RedeemMemberCouponById_ExistingLanguage()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                var reqCoupon = cdis_Service_Method.getCoupon();
                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
                Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using Id with an existing Language";
                string language = DatabaseUtility.GetFromSoapDB("Lw_Languagedef", string.Empty, string.Empty, "CULTURE", string.Empty);
                redemptiondate = System.DateTime.Now;
                var certDetails = cdis_Service_Method.RedeemMemberCouponById(long.Parse(id), "Web", language, redemptiondate, timesUsed, false, false, out elapsedTime);

                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", id, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Positive")]
        [TestMethod]
        public void BTA1236_ST1415_SOAP_RedeemMemberCouponById_CouponIdFromDb()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                var reqCoupon = cdis_Service_Method.getCoupon();
                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
                Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using Id with an Coupon Id exists in Database";
                redemptiondate = System.DateTime.Now;
                var certDetails = cdis_Service_Method.RedeemMemberCouponById(long.Parse(id), "Web", "en", redemptiondate, timesUsed, false, false, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", id, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
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

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Positive")]
        [TestMethod]
        public void BTA1236_ST1416_SOAP_RedeemMemberCouponById_ReturnAttributesFalse()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                var reqCoupon = cdis_Service_Method.getCoupon();
                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
                Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using Id where Return attributes False";
                redemptiondate = System.DateTime.Now;
                bool? returnattributes = false;
                var certDetails = cdis_Service_Method.RedeemMemberCouponById(long.Parse(id), "Web", "en", redemptiondate, timesUsed, returnattributes, false, out elapsedTime);

                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", id, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Positive")]
        [TestMethod]
        public void BTA1236_ST1417_SOAP_RedeemMemberCouponById_ReturnAttributesTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                var reqCoupon = cdis_Service_Method.getCoupon();
                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
                Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using Id where Return attributes False";
                redemptiondate = System.DateTime.Now;
                bool? returnattributes = true;
                var certDetails = cdis_Service_Method.RedeemMemberCouponById(long.Parse(id), "Web", "en", redemptiondate, timesUsed, returnattributes, false, out elapsedTime);

                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", id, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Positive")]
        [TestMethod]
        public void BTA1236_ST1418_SOAP_RedeemMemberCouponById_TimeUsed_IsOne()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                var reqCoupon = cdis_Service_Method.getCoupon();
                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
                Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using Id where Return attributes False";
                redemptiondate = System.DateTime.Now;
                timesUsed = 1;
                var certDetails = cdis_Service_Method.RedeemMemberCouponById(long.Parse(id), "Web", "en", redemptiondate, timesUsed, false, false, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", id, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Positive")]
        [TestMethod]
        public void BTA1236_ST1419_SOAP_RedeemMemberCouponById_RedemptionDateCurrent()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                var reqCoupon = cdis_Service_Method.getCoupon();
                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
                Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to a member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
                testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
                Logger.Info("MemberCoupon Id : " + memberCouponId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon using Id where Return attributes False";
                redemptiondate = System.DateTime.Now;
                timesUsed = 1;
                var certDetails = cdis_Service_Method.RedeemMemberCouponById(long.Parse(id), "Web", "en", redemptiondate, timesUsed, false, false, out elapsedTime);
                string dbRedemptionDate = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "ID", id, "DATEREDEEMED", string.Empty);
                Assert.AreEqual(System.DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"), "Expected value is" + System.DateTime.Now.ToString("MM/dd/yyyy") + " and Actual value is" + Convert.ToDateTime(dbRedemptionDate).ToString("MM/dd/yyyy"));
                testStep.SetOutput("The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   "; the redemption date is: " + dbRedemptionDate);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Coupon name from the response of RedeemMemberCouponById method is: " + certDetails.CouponDefinition.Name +
                                   " and; the redemption date is: " + dbRedemptionDate);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Negative")]
        [TestMethod]
        public void BTA1236_ST1452_SOAP_RedeemMemberCouponById_InvalidCouponId()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon with invalid coupon id and validate the Error code:3370";
                redemptiondate = System.DateTime.Now;
                string couponId = common.RandomNumber(5);
                string value = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", couponId, "CERTIFICATENMBR", string.Empty);
                while (value.Equals(couponId))
                {
                    couponId = common.RandomNumber(7);
                    value = DatabaseUtility.GetFromSoapDB("LW_MEMBERCOUPON", "CERTIFICATENMBR", couponId, "CERTIFICATENMBR", string.Empty);
                }
                string error = cdis_Service_Method.RedeemMemberCouponById_Invalid(long.Parse(couponId), "Web", "en", redemptiondate, timesUsed, false, false);
                if (error.Contains("Error code=3370") && error.Contains("Error Message=No coupon could be located using id"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3370. Actual error received is:\"" + error + "\"");
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Negative")]
        [TestMethod]
        public void BTA1236_ST1453_SOAP_RedeemMemberCouponById_InvalidLangugae()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];


                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon with invalid Language and validate the Error:6002";
                redemptiondate = System.DateTime.Now;
                string language = "test";
                string error = cdis_Service_Method.RedeemMemberCouponById_Invalid(long.Parse(id), "web", language, redemptiondate, timesUsed, false, false);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Negative")]
        [TestMethod]
        public void BTA1236_ST1454_SOAP_RedeemMemberCouponById_InvalidChannel()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWMemberCouponTable_With_EmptyDateRedeemed"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid Id from LW_MemberCoupon table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon with invalid Channel and validate the Error:6003";
                redemptiondate = System.DateTime.Now;
                string channel = "test";
                string error = cdis_Service_Method.RedeemMemberCouponById_Invalid(long.Parse(id), channel, "en", redemptiondate, timesUsed, false, false);
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
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Negative")]
        [TestMethod]
        public void BTA1236_ST1550_SOAP_RedeemMemberCouponById_WithoutCouponId()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon without passing Coupon id and validate the Error";
                redemptiondate = System.DateTime.Now;
                long couponId = 0;
                string error = cdis_Service_Method.RedeemMemberCouponById_Invalid(couponId, "Web", "en", redemptiondate, timesUsed, false, false);
                if (error.Contains("Error code=3370") && error.Contains("Error Message=No coupon could be located using id 0"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3370. Actual error received is:\"" + error + "\"");
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Negative")]
        [TestMethod]
        public void BTA1236_ST1515_SOAP_RedeemMemberCouponById_WithoutParameters()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon without passing parameters and validate the Error";
                redemptiondate = System.DateTime.Now;
                long couponId = 0;
                string error = cdis_Service_Method.RedeemMemberCouponById_Invalid(couponId, null, null, null, null, null, null);
                if (error.Contains("Error code=3370") && error.Contains("Error Message=No coupon could be located using id 0"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3370. Actual error received is:\"" + error + "\"");
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Negative")]
        [TestMethod]
        public void BTA1236_ST1549_SOAP_RedeemMemberCouponById_CouponWithUsesAllowedZero()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());

                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWCouponDefTable_Where_UsesAllowedIsZero"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid with uses allowed zero from LW_CoponDef table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id with uses allowed zero from LW_CoponDef table is: " + id);
                Logger.Info("The valid Id from LW_MemberCoupon table is: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon without passing Coupon id and validate the Error";
                redemptiondate = System.DateTime.Now;
                string error = cdis_Service_Method.RedeemMemberCouponById_Invalid(long.Parse(id), "Web", "en", redemptiondate, timesUsed, false, false);
                if (error.Contains("Error code=3370") && error.Contains("Error Message=No coupon could be located using id"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3370. Actual error received is:\"" + error + "\"");
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID")]
        [TestCategory("API_SOAP_RedeemMemberCouponByID_Negative")]
        [TestMethod]
        public void BTA1236_ST1514_SOAP_RedeemMemberCouponById_CouponIsExpired()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());

                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Id_From_LWCouponDefTable_Where_CouponIsExpired"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a expired coupon Id from LW_CouponDef table";
                string id = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);
                testStep.SetOutput("The valid Id from LW_MemberCoupon table is: " + id);
                Logger.Info("The expired coupon Id from LW_CouponDef table: " + id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member coupon without passing Coupon id and validate the Error";
                redemptiondate = System.DateTime.Now;
                string error = cdis_Service_Method.RedeemMemberCouponById_Invalid(long.Parse(id), "Web", "en", redemptiondate, timesUsed, false, false);
                if (error.Contains("Error code=3370") && error.Contains("Error Message=No coupon could be located using id"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3370. Actual error received is:\"" + error + "\"");
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
