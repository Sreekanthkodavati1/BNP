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
    [TestClass]
    public class BTA147_SOAP_DeActivatecard : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime = 0;

        [TestCategory("Regression")]
        [TestCategory("Smoke")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_DeActivateCard")]
        [TestCategory("API_SOAP_DeactivateCard_Positive")]
        [TestMethod]
        public void BTA147_ST1895_SOAP_DeActivatecard_ActiveMember_WithActiveCard()
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
                stepName = "Deactivating card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been not deactivated");
                testStep.SetOutput("Card is deactivated: " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Response from DB: LoyaltyCardId NewStatus is : \"" + dbresponse + "\" which means the card is : " + (LoyaltyCard_Status)Int32.Parse(dbresponse));
                string value = (LoyaltyCard_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
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
        [TestCategory("API_SOAP_DeActivateCard")]
        [TestCategory("API_SOAP_DeactivateCard_Positive")]
        [TestMethod]
        public void BTA1467_ST1894_SOAP_DeActivate_InactiveCard()
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
                stepName = "Deactivating the card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been deactivated");
                testStep.SetOutput("Card is deactivated: " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the card which is already deactivated for the member";
                string message1 = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual("pass", message1, "Card: " + vc[0].LoyaltyIdNumber + " has been deactivated");
                testStep.SetOutput("Card is deactivated: " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card status from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("Inactive", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Inactive and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("Card status is validated as Deactivated and the card number is " + vc[0].LoyaltyIdNumber);
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
        [TestCategory("API_SOAP_DeActivateCard")]
        [TestCategory("API_SOAP_DeactivateCard_Negative")]
        [TestMethod]
        public void BTA1467_ST1897_SOAP_DeActivate_CancelledCard()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            string errorCode = "3307";
            string expectedMessage = "Virtual card has been cancelled.  Its card canot be activated.";
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
                stepName = "Cancelling the card for the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber, out elapsedTime);
                testStep.SetOutput("The following card has Canceled :" + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
               
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the card which is Cancelled for the member";
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual(message.Contains(errorCode), true, "Error code " + errorCode + " is not returned when trying to DeActivate the Cancelled card");
                Assert.AreEqual(message.Contains(expectedMessage), true, "Message: " + expectedMessage + " is not returned when trying to DeActivate the Cancelled card");
                testStep.SetOutput(message);
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
        [TestCategory("API_SOAP_DeActivateCard")]
        [TestCategory("API_SOAP_DeactivateCard_Negative")]
        [TestMethod]
        public void BTA1467_ST8998_SOAP_DeActivate_InActiveMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            string errorCode = "3314";
            string expectedMessage = "Member is not active.  Its card canot be activated";
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
                stepName = "Deactivating the member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member is Deactivated : " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the card which is Inactive for the member";
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual(message.Contains(errorCode), true, "Error code " + errorCode + " is not returned when trying to DeActivate the Inactive member");
                Assert.AreEqual(message.Contains(expectedMessage), true, "Message: " + expectedMessage + " is not returned when trying to DeActivate the Inactive member");
                testStep.SetOutput(message);
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
        [TestCategory("API_SOAP_DeActivateCard")]
        [TestCategory("API_SOAP_DeactivateCard_Negative")]
        [TestMethod]
        public void BTA1467_ST1900_SOAP_DeActivate_ReplacedCard()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            string errorCode = "3307";
            string expectedMessage = "Virtual card has been replaced.  Its card canot be activated.";
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
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), true, System.DateTime.Now, out elapsedTime);
                vc = response.GetLoyaltyCards();
                testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the card which is Inactive for the member";
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual(message.Contains(errorCode), true, "Error code " + errorCode + " is not returned when trying to DeActivate the Replaced member");
                Assert.AreEqual(message.Contains(expectedMessage), true, "Message: " + expectedMessage + " is not returned when trying to DeActivate the Replaced member");
                testStep.SetOutput(message);
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
        [TestCategory("API_SOAP_DeActivateCard")]
        [TestCategory("API_SOAP_DeactivateCard_Negative")]
        [TestMethod]
        public void BTA1467_ST1899_SOAP_DeActivate_NonExistingCard()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            string errorCode = "3305";
            string expectedMessage = "Unable to find member with card id";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                string nonExistingCard = common.RandomNumber(7);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the card which is not existing";
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(nonExistingCard, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual(message.Contains(errorCode), true, "Error code " + errorCode + " is not returned when trying to DeActivate the non existing member");
                Assert.AreEqual(message.Contains(expectedMessage), true, "Message: " + expectedMessage + " is not returned when trying to DeActivate the non existing member");
                testStep.SetOutput(message + " for non existing card: " + nonExistingCard);
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
        [TestCategory("API_SOAP_DeActivateCard")]
        [TestCategory("API_SOAP_DeactivateCard_Positive")]
        [TestMethod]
        public void BTA1467_ST1896_SOAP_DeactivateCard_WithTranscations()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding points to members by posting transactions using UpdateMember method";
                DateTime date = DateTime.Now.AddMilliseconds(2000);
                var txnHeader = cdis_Service_Method.UpdateMember_PostTransactionRequiredDate(output, date);
                var pointBalanceBeforeCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The transaction header of the ID is  " + txnHeader.TxnHeaderId +
                    "and Currency Balance is : " + pointBalanceBeforeCancelCard.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating card for the member";
                vc = output.GetLoyaltyCards();
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been not deactivated");
                testStep.SetOutput("Card is deactivated: " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card is deactivated and points are expired";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Response from DB: LoyaltyCardId NewStatus is : \"" + dbresponse + "\" which means the card is : " + (LoyaltyCard_Status)Int32.Parse(dbresponse));
                string value = (LoyaltyCard_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
                var pointBalanceAfterDeactivateCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("0", pointBalanceAfterDeactivateCard.CurrencyBalance.ToString(), "Expected value is \"0\" and the Actual value is: " + pointBalanceAfterDeactivateCard.CurrencyBalance.ToString());
                testStep.SetOutput("Card is deactivated and the points are expired and the Card number is: " + vc[0].LoyaltyIdNumber);
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