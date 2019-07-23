using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1028_SOAP_AssociateMemberSocialHandles : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        string stepOutput = "";
        double elapsedTime = 0;
        IList<VirtualCard> vc;
        string providerType = "Facebook";
        string providerUID = "fb123";
        string updatedProviderType = "Twitter";
        string updatedProviderUID = "tw123";

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles_Positive")]
        [TestMethod]
        public void BTA1028_ST1056_SOAP_AssociateMemberSocialHandles_MemberWithNoSocialHandles()
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
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Associate social handles for a member";
                vc = output.GetLoyaltyCards();
                string msg = cdis_Service_Method.AssociateMemberSocialHandles(vc[0].LoyaltyIdNumber, providerType, providerUID, out elapsedTime);
                testStep.SetOutput(msg);
                Logger.Info(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Associate social handles for a member are added successfully";
                vc = output.GetLoyaltyCards();
                MemberSocialHandleStruct[] socialHandlesData = cdis_Service_Method.GetMemberSocialHandles(vc[0].LoyaltyIdNumber, out stepOutput);
                Assert.AreEqual(socialHandlesData[0].ProviderType, providerType, "Expected value is " + providerType + "actual value is " + socialHandlesData[0].ProviderType);
                Assert.AreEqual(socialHandlesData[0].ProviderUID, providerUID, "Expected value is " + providerUID + "actual value is" + socialHandlesData[0].ProviderUID);
                testStep.SetOutput("Social handles for a member are added successfully where loyaltyidnumber is: " + vc[0].LoyaltyIdNumber + " ,ProviderType: " + socialHandlesData[0].ProviderType +
                    " and ProviderUID: " + socialHandlesData[0].ProviderUID);
                Logger.Info("Social handles for a member are added successfully where loylaty id is: " + vc[0].LoyaltyIdNumber + "ProviderType" + socialHandlesData[0].ProviderType +
                    " and ProviderUID" + socialHandlesData[0].ProviderUID);
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

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles_Positive")]
        [TestMethod]
        public void BTA1028_ST1058_SOAP_UpdateMemberSocialHandles_UpdateSocialHandlesExistingSocialHandles()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            string msg = "";

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Associate social handles for a member";
                vc = output.GetLoyaltyCards();
                msg = cdis_Service_Method.AssociateMemberSocialHandles(vc[0].LoyaltyIdNumber, providerType, providerUID, out elapsedTime);
                testStep.SetOutput(msg);
                Logger.Info(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Update social handles for a member";
                vc = output.GetLoyaltyCards();
                msg = cdis_Service_Method.AssociateMemberSocialHandles(vc[0].LoyaltyIdNumber, updatedProviderType, updatedProviderUID, out elapsedTime);
                testStep.SetOutput(msg);
                Logger.Info(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Associate social handles for a member are updated successfully";
                vc = output.GetLoyaltyCards();
                MemberSocialHandleStruct[] socialHandlesData = cdis_Service_Method.GetMemberSocialHandles(vc[0].LoyaltyIdNumber, out stepOutput);
                Assert.AreEqual(socialHandlesData[0].ProviderType, updatedProviderType, "Expected value is: " + updatedProviderUID + " actual value is: " + socialHandlesData[0].ProviderType);
                Assert.AreEqual(socialHandlesData[0].ProviderUID, updatedProviderUID, "Expected value is: " + updatedProviderUID + " actual value is: " + socialHandlesData[0].ProviderUID);
                testStep.SetOutput("Social handles for a member are updated successfully where loyaltyid is: " + vc[0].LoyaltyIdNumber + " ,ProviderType: " + socialHandlesData[0].ProviderType +
                    " and ProviderUID: " + socialHandlesData[0].ProviderUID);
                Logger.Info("Social handles for a member are added updated successfully where loyaltyid is: " + vc[0].LoyaltyIdNumber + "ProviderType: " + socialHandlesData[0].ProviderType +
                    "and ProviderUID: " + socialHandlesData[0].ProviderUID);
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

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles_Positive")]
        [TestMethod]
        public void BTA1028_ST1172_SOAP_AssociateMemberSocialHandles_verifyElapsedTime()
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
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Associate social handles for a member";
                vc = output.GetLoyaltyCards();
                string msg = cdis_Service_Method.AssociateMemberSocialHandles(vc[0].LoyaltyIdNumber, providerType, providerUID, out elapsedTime);
                testStep.SetOutput(msg);
                Logger.Info(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Associate social handles for a member are added successfully";
                vc = output.GetLoyaltyCards();
                MemberSocialHandleStruct[] socialHandlesData = cdis_Service_Method.GetMemberSocialHandles(vc[0].LoyaltyIdNumber, out stepOutput);
                Assert.AreEqual(socialHandlesData[0].ProviderType, providerType, "Expected value is " + providerType + "actual value is " + socialHandlesData[0].ProviderType);
                Assert.AreEqual(socialHandlesData[0].ProviderUID, providerUID, "Expected value is " + providerUID + "actual value is" + socialHandlesData[0].ProviderUID);
                testStep.SetOutput("Social handles for a member are added successfully where loyaltyid is: " + vc[0].LoyaltyIdNumber + " ,ProviderType: " + socialHandlesData[0].ProviderType +
                    " and ProviderUID: " + socialHandlesData[0].ProviderUID);
                Logger.Info("Social handles for a member are added successfully where loylaty id is: " + vc[0].LoyaltyIdNumber + "ProviderType" + socialHandlesData[0].ProviderType +
                    " and ProviderUID" + socialHandlesData[0].ProviderUID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the elapsed time is greater than Zero";
                if (elapsedTime > 0)
                {
                    testStep.SetOutput("Elapsed time is greater than zero and the actual elapsed time is " + elapsedTime);
                }
                else
                {
                    throw new Exception("Elapsed time is not greater than Zero");
                }
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

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles_Negative")]
        [TestMethod]
        public void BTA1028_ST1059_SOAP_AssociateMemberSocialHandles_NonExistingMemberIdentity()
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
                stepName = "Associate social handles for a member not existing in Database and validate the Error Code as 3302";
                string memberIdentity = "";
                memberIdentity = common.RandomNumber(7);
                string value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                while (value == memberIdentity)
                {
                    memberIdentity = common.RandomNumber(7);
                    value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                }
                string error = cdis_Service_Method.AssociateMemberSocialHandles(memberIdentity, providerType, providerUID, out elapsedTime);
                if (error.Contains("3302") && error.Contains("Unable to find member with identity"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected and it is: " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:3302. Actual error received is" + error);
                }
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

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles_Negative")]
        [TestMethod]
        public void BTA1028_ST1060_SOAP_AssociateMemberSocialHandles_NonExistingProviderType()
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
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Associate social handles for a member with Invalid Provider type and validate the Error Code as 3397";
                vc = output.GetLoyaltyCards();
                string outMsg = cdis_Service_Method.AssociateMemberSocialHandles(vc[0].LoyaltyIdNumber, "TestProvider", providerUID, out elapsedTime);
                testStep.SetOutput(outMsg);
                if (outMsg.Contains("Error code=3397"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected and it is: " + outMsg);
                    Logger.Info("The Error message from Service is received as expected. " + outMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error:3397. Actual error received is" + outMsg);
                }
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

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles_Negative")]
        [TestMethod]
        public void BTA1028_ST1173_SOAP_AssociateMemberSocialHandles_ProviderTypeNullProviderUIDNull()
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
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Associate social handles for a member with Invalid Provider type and validate the Error Code as 2003";
                vc = output.GetLoyaltyCards();
                string outMsg = cdis_Service_Method.AssociateMemberSocialHandles(vc[0].LoyaltyIdNumber, null, null, out elapsedTime);
                testStep.SetOutput(outMsg);
                if (outMsg.Contains("Error code=2003") && outMsg.Contains("Error Message=ProviderType of MemberSocialHandleStruct is a required property."))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + outMsg);
                    Logger.Info("The Error message from Service is received as expected. " + outMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error:2003. Actual error received is" + outMsg);
                }
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

        [TestCategory("API_SOAP_Regression")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles")]
        [TestCategory("API_SOAP_AssociateMemberSocialHandles_Negative")]
        [TestMethod]
        public void BTA1028_ST1249_SOAP_AssociateMemberSocialHandles_WithMemberIdentity()
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
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Associate social handles for a member with only memberIdentity and validate the Error Code as 2003";
                vc = output.GetLoyaltyCards();
                string outMsg = cdis_Service_Method.AssociateMemberSocialHandles(vc[0].LoyaltyIdNumber, null, null, out elapsedTime);
                testStep.SetOutput(outMsg);
                if (outMsg.Contains("Error code=2003") && outMsg.Contains("Error Message=ProviderType of MemberSocialHandleStruct is a required property."))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + outMsg);
                    Logger.Info("The Error message from Service is received as expected. " + outMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error:2003. Actual error received is" + outMsg);
                }
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