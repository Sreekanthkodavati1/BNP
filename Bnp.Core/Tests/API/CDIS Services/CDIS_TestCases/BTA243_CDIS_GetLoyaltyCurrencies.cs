using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA243_CDIS_GetLoyaltyCurrencies : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA243_CDIS_GetLoyaltyCurrencies_PositiveCase()

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
                stepName = "Get Loyalty Currency types from GetLoyalityCurrencies method";
                LoyaltyCurrencyStruct[] currencyStruct = cdis_Service_Method.GetLoyalityCurrencies();
                testStep.SetOutput("Types of Loyalty Currencies returned by method are :;" + currencyStruct[0].Name + ";" + currencyStruct[1].Name);
                Logger.Info("Types of Loyalty Currencies returned by method are :" + currencyStruct[0].Name + " and " + currencyStruct[1].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the loyalty currencies types in the DB in LW_POINTTYPE table";
                List<string>dbresponse = DatabaseUtility.GetLoyalityCurrenciesfromDBUsingIdSOAP();
                for(int i=0; i<currencyStruct.Length;i++)
                {
                    Assert.AreEqual(currencyStruct[i].Name + "", dbresponse[i], "Expected Value is " + currencyStruct[i].Name + " Actual Value is " + dbresponse[i]);
                }
                testStep.SetOutput("The loyalty currencies types from the DB are :; " + dbresponse[0]+";"+ dbresponse[1]);
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
        }
    }
