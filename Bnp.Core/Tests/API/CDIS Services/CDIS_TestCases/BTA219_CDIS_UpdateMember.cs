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
    public class BTA219_CDIS_UpdateMember : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA219_CDIS_UpdateMember_AddTransaction_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Updating Member by adding transactions";
                DateTime date = DateTime.Now.AddDays(-5);
                string txnHeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(output, date);
                testStep.SetOutput("The transaction header of the transcation posted through udpatemember method is  " + txnHeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetTxnHeaderIdusingVCKeyDBSoap(vc[0].VcKey+"");
                Assert.AreEqual(txnHeaderId, dbresponse, "Expected value is" + txnHeaderId + "Actual value is" + dbresponse);
                testStep.SetOutput("TXNheaderID value from db is: " + dbresponse+" and TXNheaderID from service response is: "+txnHeaderId);
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

		[TestMethod]
		public void BTA219_CDIS_UpdateMember_UpdateEmailId_Positive()
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
				Member output = cdis_Service_Method.AddSoapMember(common.AddMemberWithAllFields());
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName+" and members email is : \"["+ output.PrimaryEmailAddress + "]\"");
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Updating Member email through UpdateMember method";
				output.PrimaryEmailAddress = output.FirstName + "@updatemethod.com";
				Member updatedMember = cdis_Service_Method.UpdateMemberGeneral(output);
				testStep.SetOutput("The members primary emailID is updated to : " + updatedMember.PrimaryEmailAddress);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the member emailid from database";
				string dbresponse = DatabaseUtility.GetEmailIDfromDBSOAP(updatedMember.IpCode+"");
				Assert.AreEqual(output.PrimaryEmailAddress, dbresponse, "Expected value is" + output.PrimaryEmailAddress + "Actual value is" + dbresponse);
				testStep.SetOutput("EmailId value from db is :" + dbresponse + " and EmailID updated from udpateMethod is :" + output.PrimaryEmailAddress);
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
