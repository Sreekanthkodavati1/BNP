using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA990_CDIS_AuthenticateMember : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double time = 0;

        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("Regression")]
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Positive")]
        [TestMethod]
        public void BTA990_SOAP_AuthenticateMember()
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
                Member member = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + member.IpCode + ", UserName: " + member.Username);
                Logger.Info("IpCode: " + member.IpCode + ", Name: " + member.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Authenticating a Member using AuthenticateMember method";
                var output = cdis_Service_Method.AuthenticateMember("username", member.Username, "Password1*", string.Empty, out time);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " from the response");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the response of Authenticate Member";
                Assert.AreEqual("True", output.Authenticated.ToString(), "Expected value is True and the Actual value is" + output.Authenticated.ToString());
                Assert.AreEqual("Success", output.LoginStatus, "Expected value is Success and the Actual value is" + output.LoginStatus);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " are verified from the response");
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
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Positive")]
        [TestMethod]
        public void BTA1032_ST1251_SOAP_AuthenticateMember_VerifyElapsedTime()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double time = 0;

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member member = cdis_Service_Method.AddCDISMemberWithAllFields();
                testStep.SetOutput("IpCode: " + member.IpCode + ", UserName: " + member.Username);
                Logger.Info("IpCode: " + member.IpCode + ", Name: " + member.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Authenticating a Member and verifying the elapsed time";
                var output = cdis_Service_Method.AuthenticateMember("PrimaryEmailAddress", member.PrimaryEmailAddress, "Password1*", string.Empty, out time);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " from the response with elapsed time " + time);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the response of Authenticate Member";
                Assert.AreEqual("True", output.Authenticated.ToString(), "Expected value is True and the Actual value is" + output.Authenticated.ToString());
                Assert.AreEqual("Success", output.LoginStatus, "Expected value is Success and the Actual value is" + output.LoginStatus);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " are verified from the response");
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
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Positive")]
        [TestMethod]
        public void BTA1032_ST1252_SOAP_AuthenticateMember_IdentityType_LoyaltyIdNumber()
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
                Member member = cdis_Service_Method.AddCDISMemberWithAllFields();
                testStep.SetOutput("IpCode: " + member.IpCode + ", UserName: " + member.Username);
                Logger.Info("IpCode: " + member.IpCode + ", Name: " + member.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = member.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Authenticating a Member with identity type as LoyaltyId number";
                var output = cdis_Service_Method.AuthenticateMember("LoyaltyIdNumber", vc[0].LoyaltyIdNumber, "Password1*", string.Empty, out time);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " , LoginStatus: " + output.LoginStatus + " from the response");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the response of Authenticate Member";
                Assert.AreEqual("True", output.Authenticated.ToString(), "Expected value is True and the Actual value is" + output.Authenticated.ToString());
                Assert.AreEqual("Success", output.LoginStatus, "Expected value is Success and the Actual value is" + output.LoginStatus);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " are verified from the response");
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
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Positive")]
        [TestMethod]
        public void BTA1032_ST1253_SOAP_AuthenticateMember_IdentityType_PrimaryEmailAddress()
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
                Member member = cdis_Service_Method.AddCDISMemberWithAllFields();
                testStep.SetOutput("IpCode: " + member.IpCode + ", UserName: " + member.Username);
                Logger.Info("IpCode: " + member.IpCode + ", Name: " + member.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Authenticating a Member with identity type as PrimaryEmailAddress";
                var output = cdis_Service_Method.AuthenticateMember("PrimaryEmailAddress", member.PrimaryEmailAddress, "Password1*", string.Empty, out time);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " , LoginStatus: " + output.LoginStatus + " from the response");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the response of Authenticate Member";
                Assert.AreEqual("True", output.Authenticated.ToString(), "Expected value is True and the Actual value is" + output.Authenticated.ToString());
                Assert.AreEqual("Success", output.LoginStatus, "Expected value is Success and the Actual value is" + output.LoginStatus);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " are verified from the response");
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
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Positive")]
        [TestMethod]
        public void BTA1032_ST1254_SOAP_AuthenticateMember_IdentityType_UserName()
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
                Member member = cdis_Service_Method.AddCDISMemberWithAllFields();
                testStep.SetOutput("IpCode: " + member.IpCode + ", UserName: " + member.Username);
                Logger.Info("IpCode: " + member.IpCode + ", Name: " + member.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Authenticating a Member with identity type as UserName";
                var output = cdis_Service_Method.AuthenticateMember("UserName", member.Username, "Password1*", string.Empty, out time);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " , LoginStatus: " + output.LoginStatus + " from the response");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the response of Authenticate Member";
                Assert.AreEqual("True", output.Authenticated.ToString(), "Expected value is True and the Actual value is" + output.Authenticated.ToString());
                Assert.AreEqual("Success", output.LoginStatus, "Expected value is Success and the Actual value is" + output.LoginStatus);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " are verified from the response");
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
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Negative")]
        [TestMethod]
        public void BTA1032_ST1264_SOAP_AuthenticateMember_IdentityNotExists()
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
                stepName = "Authenticating a Member with identity not exists and validate the error message";
                string Username = common.RandomString(10);
                var output = cdis_Service_Method.AuthenticateMember("username", Username, "Password1*", string.Empty, out time);
                Assert.AreEqual("Authentication error:Unable to find " + "'" + Username + "'" + " (Username)", output.StatusText, "Actual value is " + output.StatusText + "Expected value is Authentication error: Unable to find (Username)");
                testStep.SetOutput("The Error message from Service is received as expected. " + output.StatusText);
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
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Negative")]
        [TestMethod]
        public void BTA1032_ST1265_SOAP_AuthenticateMember_IdentityTypeNotExists()
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
                stepName = "Authenticating a Member with identity type not exists and validate the Error as 3304";
                string memberIdentity = "";
                memberIdentity = common.RandomNumber(7);
                string value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                while (value == memberIdentity)
                {
                    memberIdentity = common.RandomNumber(7);
                    value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                }
                var output = cdis_Service_Method.AuthenticateMemberInvalid("invalidIdentity", memberIdentity, "Password1*", string.Empty);
                if (output.Contains("Error code=3304") && output.Contains("Error Message=Invalid identityType 'invalidIdentity' provided for authenticate member"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + output);
                    Logger.Info("The Error message from Service is received as expected. " + output);
                }
                else
                {
                    throw new Exception("Error not received as expected error:3004. Actual error received is" + output);
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
        [TestCategory("API_SOAP_AuthenticateMember")]
        [TestCategory("API_SOAP_AuthenticateMember_Negative")]
        [TestMethod]
        public void BTA1032_ST1270_SOAP_AuthenticateMember_WrongPassword()
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
                Member member = cdis_Service_Method.AddCDISMemberWithAllFields();
                testStep.SetOutput("IpCode: " + member.IpCode + ", UserName: " + member.Username);
                Logger.Info("IpCode: " + member.IpCode + ", Name: " + member.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Authenticating a Member with identity type as UserName";
                var output = cdis_Service_Method.AuthenticateMember("UserName", member.Username, "Password1*12", string.Empty, out time);
            //    Assert.AreEqual("Authentication error:Provided password is incorrect", output.StatusText, "Actual value is " + output.StatusText + "Expected value is Authentication error:Provided password is incorrect");
                testStep.SetOutput("The login status from Service is received as expected. " + output.LoginStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the response of Authenticate Member";
                Assert.AreEqual("False", output.Authenticated.ToString(), "Expected value is True and the Actual value is" + output.Authenticated.ToString());
                Assert.AreEqual("Failure", output.LoginStatus, "Expected value is Success and the Actual value is" + output.LoginStatus);
                testStep.SetOutput("Authenticated: " + output.Authenticated + " and LoginStatus: " + output.LoginStatus + " are verified from the response");
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
