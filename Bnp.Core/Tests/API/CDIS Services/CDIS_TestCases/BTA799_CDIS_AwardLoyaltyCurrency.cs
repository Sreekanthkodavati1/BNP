using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
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
    public class BTA799_CDIS_AwardLoyaltyCurrency: ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_AwardLoyaltyCurrency")]
        [TestCategory("API_SOAP_AwardLoyaltyCurrency_Positive")]
        [TestMethod]
        public void BTA799_SOAP_AwardLoyaltyCurrency()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method";
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints);
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with the currency balance returned by GetAccountSummary method";
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(getAccountSummary.CurrencyBalance, awardLoyaltyCurrency.CurrencyBalance, "Expected value is" + getAccountSummary.CurrencyBalance + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance);
                testStep.SetOutput("The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance + "" +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with DB";
                string dbresponse = DatabaseUtility.GetLoyaltyCurrencyBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                Assert.AreEqual(dbresponse, awardLoyaltyCurrency.CurrencyBalance.ToString(), "Expected value is" + dbresponse + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep.SetOutput("The LoyaltyCurrencyBalance from DB is: " + dbresponse + "" +
                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from DB is: " + dbresponse +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
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
        [TestCategory("API_SOAP_AwardLoyaltyCurrency")]
        [TestCategory("API_SOAP_AwardLoyaltyCurrency_Positive")]
        [TestMethod]
        public void BTA1230_ST1387_SOAP_AwardLoyaltyCurrency_blankTransactionDate()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();
            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method where transaction date is null";
                decimal amount = Convert.ToDecimal(common.RandomNumber(3));
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = null;
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "SOAPService");
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with the currency balance returned by GetAccountSummary method";
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(getAccountSummary.CurrencyBalance, awardLoyaltyCurrency.CurrencyBalance, "Expected value is" + getAccountSummary.CurrencyBalance + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance);
                testStep.SetOutput("The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance + "" +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with DB";
                string dbresponse = DatabaseUtility.GetLoyaltyCurrencyBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                Assert.AreEqual(dbresponse, awardLoyaltyCurrency.CurrencyBalance.ToString(), "Expected value is" + dbresponse + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep.SetOutput("The LoyaltyCurrencyBalance from DB is: " + dbresponse + "" +
                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from DB is: " + dbresponse +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
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
        [TestCategory("API_SOAP_AwardLoyaltyCurrency")]
        [TestCategory("API_SOAP_AwardLoyaltyCurrency_Negative")]
        [TestMethod]
        public void BTA1230_ST1388_AwardLoyaltyCurrency_CurrencyAmountZero()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method where currency amount is 0";
                decimal? amount = 0;
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now;
                string errorMsg = cdis_Service_Method.AwardLoyaltyCurrencyInvalid(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "SOAPService");
                if (errorMsg.Contains("Error code=3313") && errorMsg.Contains("Error Message=0 points were requested.  Loyalty currency canot be 0"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3362. Actual error received is" + errorMsg);
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
        [TestCategory("API_SOAP_AwardLoyaltyCurrency")]
        [TestCategory("API_SOAP_AwardLoyaltyCurrency_Negative")]
        [TestMethod]
        public void BTA1230_ST1389_AwardLoyaltyCurrency_InvalidMemberIdentity()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                decimal amount = Convert.ToDecimal(common.RandomNumber(3));
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now;
                string memberIdentity = common.RandomNumber(7);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method where member identity not exists";
                string value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                while (value == memberIdentity)
                {
                    memberIdentity = common.RandomNumber(7);
                    value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                }
                string errorMsg = cdis_Service_Method.AwardLoyaltyCurrencyInvalid(memberIdentity, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "CDISService");
                if (errorMsg.Contains("Error code=3305") && errorMsg.Contains("Error Message=Unable to find member with loyalty id"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3362. Actual error received is" + errorMsg);
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
        [TestCategory("API_SOAP_AwardLoyaltyCurrency")]
        [TestCategory("API_SOAP_AwardLoyaltyCurrency_Positive")]
        [TestMethod]
        public void BTA1230_ST1512_SOAP_AwardLoyaltyCurrency_NegativeWholeNumber()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                decimal amount = -10;
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now; ;

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency with a negative whole number";
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "CDISService");
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with the currency balance returned by GetAccountSummary method";
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(getAccountSummary.CurrencyBalance, awardLoyaltyCurrency.CurrencyBalance, "Expected value is" + getAccountSummary.CurrencyBalance + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance);
                testStep.SetOutput("The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance + "" +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with DB";
                string dbresponse = DatabaseUtility.GetLoyaltyCurrencyBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                Assert.AreEqual(dbresponse, awardLoyaltyCurrency.CurrencyBalance.ToString(), "Expected value is" + dbresponse + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep.SetOutput("The LoyaltyCurrencyBalance from DB is: " + dbresponse + "" +
                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from DB is: " + dbresponse +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
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
        [TestCategory("API_SOAP_AwardLoyaltyCurrency")]
        [TestCategory("API_SOAP_AwardLoyaltyCurrency_Positive")]
        [TestMethod]
        public void BTA1230_ST1513_SOAP_AwardLoyaltyCurrency_BlankCurrencyAmount()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                decimal? amount = null;
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now; 

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method with blank currency amount";
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "CDISService");
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with the currency balance returned by GetAccountSummary method";
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(getAccountSummary.CurrencyBalance, awardLoyaltyCurrency.CurrencyBalance, "Expected value is" + getAccountSummary.CurrencyBalance + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance);
                testStep.SetOutput("The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance + "" +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from GetAccountSummary method is: " + getAccountSummary.CurrencyBalance +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the LoyaltyCurrency balance with DB";
                string dbresponse = DatabaseUtility.GetLoyaltyCurrencyBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                Assert.AreEqual(dbresponse, awardLoyaltyCurrency.CurrencyBalance.ToString(), "Expected value is" + dbresponse + "Actual value is" + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep.SetOutput("The LoyaltyCurrencyBalance from DB is: " + dbresponse + "" +
                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance from DB is: " + dbresponse +
                                   " and the LoyaltyCurrencyBalance from AwardLoyaltyCurrency method is: " + awardLoyaltyCurrency.CurrencyBalance);
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
