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

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA90_CDIS_GetMemberById: ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        [TestMethod]
        public void BTA90_CDIS_GetMemberById_PositiveCase()
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
                stepName = "Getting member by Id with CDIS service";
                Member member = cdis_Service_Method.GetCDISMemberGeneral();
                Member[] output = cdis_Service_Method.GetCDISMemberById(member.IpCode + "");
                Logger.Info("IpCode:" + output[0].IpCode + ",Name:" + output[0].FirstName);
                testStep.SetOutput("IpCode:" + output[0].IpCode + ",Name:" + output[0].FirstName);
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
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }

        //[TestMethod]
        //public void API_CDIS_getMemberByCardId()

        //{
        //	testCase = new TestCase(TestContext.TestName);
        //	listOfTestSteps = new List<TestStep>();
        //	testStep = new TestStep();
        //	String stepName = "";
        //	try
        //	{
        //		Logger.Info("Test Method Started");
        //		Common common = new Common(this.DriverContext);
        //		CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
        //		testStep = TestStepHelper.StartTestStep(testStep);
        //		stepName = "Getting member by Id with CDIS service";
        //		Member member = cdis_Service_Method.getCDISMemberGeneral();
        //		IList<VirtualCard> vclist = member.GetLoyaltyCards();
        //		Member[] output = cdis_Service_Method.getCDISMemberByCardId(vclist[0].LoyaltyIdNumber + "");
        //		Logger.Info("IpCode:" + output[0].IpCode + ",Name:" + output[0].FirstName);

        //		testStep.SetOutput("IpCode:" + output[0].IpCode + ",Name:" + output[0].FirstName);
        //		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent());
        //		listOfTestSteps.Add(testStep);
        //		testCase.SetStatus(true);
        //	}
        //	catch (Exception e)
        //	{
        //		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent());
        //		listOfTestSteps.Add(testStep);
        //		testCase.SetStatus(false);
        //		testCase.SetErrorMessage(e.Message);
        //	}
        //	finally
        //	{
        //		testCase.SetTestCaseSteps(listOfTestSteps);
        //		testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
        //		listOfTestCases.Add(testCase);
        //	}
        //}
    }
}