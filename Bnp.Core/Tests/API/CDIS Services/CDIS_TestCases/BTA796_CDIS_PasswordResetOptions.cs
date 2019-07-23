using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using System.Collections.Generic;
using System;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA796_CDIS_PasswordResetOptions : ProjectTestBase
    {

        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA796_CDIS_PasswordResetOptions_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.AddCDISMemberWithAllFields();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();


       //         MemberDetails memberdet = new MemberDetails();
        //        var test = memberdet.MobilePhone;

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get PasswordReset Options for a member using LoyaltyIDNumber";
                var response = cdis_Service_Method.GetPasswordResetOptionsUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The Masked Email and SMS from the response are :"+ response.Email+ " and " + response.SMS);
                Logger.Info("TestStep: " + stepName + " ##Passed## The Masked Email and SMS from the response are :" + response.Email + " and " + response.SMS);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the masked Email and SMS using Addmember response and ATS_Memberdetails table";
                var maskedEmail = response.Email;
                string [] Emailnew = maskedEmail.Split('@');
                var maskedPhone = response.SMS;
                string dbresponse = DatabaseUtility.GetFromSoapDB("ATS_MEMBERDETAILS", "A_IPCODE", output.IpCode.ToString(), "A_MOBILEPHONE",string.Empty);
                var expectedPhone = dbresponse.Replace(dbresponse.Substring(3, 5), "?????");
                Assert.IsTrue(output.PrimaryEmailAddress.Contains(Emailnew[0].Replace("?", "").Trim()));
                Assert.AreEqual(expectedPhone, maskedPhone, "Expected value is" + expectedPhone + "Actual value is" + maskedPhone);
                testStep.SetOutput("The expected Email is:; "+ output.PrimaryEmailAddress + " ;and actual masked email is :" + response.Email
                                    + ";The expected masked Phonenumber is: " + expectedPhone + " ;and actual masked Phone is :" + response.SMS);
                Logger.Info("TestStep: " + stepName + " ##Passed## The expected Email is:; " + output.PrimaryEmailAddress + " ;and actual masked email is :" + response.Email
                                    + ";The expected masked Phonenumber is: " + expectedPhone + " ;and actual masked Phone is :" + response.SMS);
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
