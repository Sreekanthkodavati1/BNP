using System;
using System.Collections.Generic;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    /// <summary>
    /// Summary description for BTA149_CDIS_ReplaceCard
    /// </summary>
    [TestClass]
    public class BTA149_CDIS_ReplaceCard : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double time = 0;

        [TestCategory("Smoke")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Positive")]
        [TestMethod]
        public void BTA149_ST1768_SOAP_ReplaceCardForMember()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), true, System.DateTime.Now, out time);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Asserting the Response";
                Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
                Assert.AreEqual(vc[1].Status.ToString(), "Active");
                testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardCountfromDbSOAP(response.IpCode + "");
                Assert.AreEqual(vc.Count + "", dbresponse, "Expected value is" + vc.Count.ToString() + "Actual value is" + dbresponse);
                testStep.SetOutput("Total number of active and inactive cards for the current member are : " + dbresponse);
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
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Positive")]
        [TestMethod]
        public void BTA1468_ST1769_SOAP_ReplaceCard_FutureDate()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime Datenew = DateTime.Now.AddMonths(2);
                bool transferPoints = true;
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), transferPoints, Datenew, out time);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Asserting the Response";
                Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
                Assert.AreEqual(vc[1].Status.ToString(), "Active");
                testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardCountfromDbSOAP(response.IpCode + "");
                Assert.AreEqual(vc.Count + "", dbresponse, "Expected value is" + vc.Count.ToString() + "Actual value is" + dbresponse);
                testStep.SetOutput("Total number of active and inactive cards for the current member are : " + dbresponse + " and the Elapsed time is:" + time + "ms");
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
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Positive")]
        [TestMethod]
        public void BTA1468_ST1770_SOAP_ReplaceCard_PastDate()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime Datenew = DateTime.Now.AddMonths(-2);
                bool transferPoints = true;
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), transferPoints, Datenew, out time);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Asserting the Response";
                Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
                Assert.AreEqual(vc[1].Status.ToString(), "Active");
                testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardCountfromDbSOAP(response.IpCode + "");
                Assert.AreEqual(vc.Count + "", dbresponse, "Expected value is" + vc.Count.ToString() + "Actual value is" + dbresponse);
                testStep.SetOutput("Total number of active and inactive cards for the current member are : " + dbresponse + " and the Elapsed time is:" + time + "ms");
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
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Positive")]
        [TestMethod]
        public void BTA1468_ST1771_SOAP_ReplaceCard_TransactionPoints_False()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate member points in DB";
                string InitialCardPoints = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                testStep.SetOutput("LoyaltyCurrencyBalance value from db is: " + InitialCardPoints);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                DateTime Datenew = DateTime.Now.AddMilliseconds(2000);
                bool transferPoints = false;
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), transferPoints, Datenew, out time);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Asserting the Response";
                Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
                Assert.AreEqual(vc[1].Status.ToString(), "Active");
                testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate member points in DB";
                string ReplacedCardPoints = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[1].VcKey + "");
                Assert.AreNotEqual(ReplacedCardPoints, InitialCardPoints, "Expected value is" + ReplacedCardPoints + "Actual value is" + InitialCardPoints);
                testStep.SetOutput("LoyaltyCurrencyBalance value from db is: " + ReplacedCardPoints);
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
        [TestCategory("API_SOAP_ReplaceCardForMember")]
        [TestCategory("ReplaceCardForMemberForTransactionPoints_Positive")]
        [TestMethod]
        public void BTA1468_ST1772_SOAP_ReplaceCard_TransactionPoints_True()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate member points in DB";
                string InitialCardPoints = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                testStep.SetOutput("LoyaltyCurrencyBalance value from db is: " + InitialCardPoints);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                DateTime Datenew = DateTime.Now.AddMilliseconds(2000);
                bool transferPoints = true;
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), transferPoints, Datenew, out time);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Asserting the Response";
                Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
                Assert.AreEqual(vc[1].Status.ToString(), "Active");
                testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate member points in DB";
                string ReplacedCardPoints = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[1].VcKey + "");
                Assert.AreEqual(ReplacedCardPoints, InitialCardPoints, "Expected value is" + ReplacedCardPoints + "Actual value is" + InitialCardPoints);
                testStep.SetOutput("LoyaltyCurrencyBalance value from db is: " + ReplacedCardPoints);
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
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Positive")]
        [TestMethod]
        public void BTA1468_ST1773_SOAP_ReplaceCard_TransactionPoints_Null()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime Datenew = DateTime.Now;
                bool? transferPoints = null;
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), transferPoints, Datenew, out time);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Asserting the Response";
                Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
                Assert.AreEqual(vc[1].Status.ToString(), "Active");
                testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardCountfromDbSOAP(response.IpCode + "");
                Assert.AreEqual(vc.Count + "", dbresponse, "Expected value is" + vc.Count.ToString() + "Actual value is" + dbresponse);
                testStep.SetOutput("Total number of active and inactive cards for the current member are : " + dbresponse + " and the Elapsed time is:" + time + "ms");
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
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Negative")]
        [TestMethod]
        public void BTA1468_ST1774_SOAP_ReplaceCard_ByPassingInvalidCardId()
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
                stepName = "Replacing card for the member";
                DateTime Datenew = DateTime.Now;
                bool? transferPoints = true;
                string response = (string)cdis_Service_Method.ReplaceCardNegative(common.RandomNumber(7), common.RandomNumber(7), transferPoints, Datenew);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');

                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("3305", errorssplit[1], "Expected value is" + "3305" + "Actual value is" + errorssplit[1]);
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
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Negative")]
        [TestMethod]
        public void BTA1468_ST1775_SOAP_ReplaceCardAnotherMemberCardId()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output1 = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName);
                Logger.Info("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                IList<VirtualCard> vc1 = output1.GetLoyaltyCards();
                DateTime Datenew = DateTime.Now;
                bool transferPoints = true;
                string response = (string)cdis_Service_Method.ReplaceCardNegative(vc[0].LoyaltyIdNumber, vc1[0].LoyaltyIdNumber, transferPoints, Datenew);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');
                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("9967", errorssplit[1], "Expected value is" + "9967" + "Actual value is" + errorssplit[1]);
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
        [TestCategory("API_SOAP_ReplaceCard")]
        [TestCategory("API_SOAP_ReplaceCard_Negative")]
        [TestMethod]
        public void BTA1468_ST1776_SOAP_ReplaceCard_WithSameMember_AlreadyExists()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                DateTime Datenew = DateTime.Now;
                bool transferPoints = true;
                string response = (string)cdis_Service_Method.ReplaceCardNegative(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, transferPoints, Datenew);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');
                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("3226", errorssplit[1], "Expected value is" + "3226" + "Actual value is" + errorssplit[1]);
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
        [TestCategory("API_SOAP_ReplaceCardForMember")]
        [TestCategory("ReplaceCardForMemberForTransactionPoints_Positive")]
        [TestMethod]
        public void BTA1468_ST1956_SOAP_ReplaceCard_TransactionPoints_VerifyTransferredStatus()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate member points in DB";
                string InitialCardPoints = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                testStep.SetOutput("LoyaltyCurrencyBalance value from db is: " + InitialCardPoints);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replacing card for the member";
                DateTime Datenew = DateTime.Now.AddMilliseconds(2000);
                bool transferPoints = true;
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), transferPoints, Datenew, out time);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Asserting the Response";
                Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
                Assert.AreEqual(vc[1].Status.ToString(), "Active");
                testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate member points in DB";
                string ReplacedCardPoints = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[1].VcKey + "");
                string transType = DatabaseUtility.GetFromSoapDB("LW_POINTTRANSACTION", "VCKEY", vc[0].VcKey.ToString()+ "' and TRANSACTIONTYPE = '5", "TRANSACTIONTYPE", string.Empty);
                Assert.AreEqual("5", transType, "Expected value is 5 and Actual value is" + transType);
                Assert.AreEqual(ReplacedCardPoints, InitialCardPoints, "Expected value is" + ReplacedCardPoints + "Actual value is" + InitialCardPoints);
                testStep.SetOutput("LoyaltyCurrencyBalance value from db is: " + ReplacedCardPoints+" and transactiontype is: "+ transType+"(transferred)");
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