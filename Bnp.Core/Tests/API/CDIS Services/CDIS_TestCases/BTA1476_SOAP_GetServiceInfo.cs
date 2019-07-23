using System;
using System.Collections.Generic;
using System.Configuration;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.Models;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1476_SOAP_GetServiceInfo : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        //readonly RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
        double elapsedTime=0;
        TestStep testStep;
        string environment = ConfigurationManager.AppSettings["Environment"];
        string organization = ConfigurationManager.AppSettings["Organization"];
        string loyaltyWareAssemblyVersion = ConfigurationManager.AppSettings["LoyaltyWareAssemblyVersion"];
        string loyaltyWareSchemaVersion = ConfigurationManager.AppSettings["LoyaltyWareSchemaVersion"];
        string cdisAssemblyVersion = ConfigurationManager.AppSettings["CDISAssemblyVersion"];

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetServiceInfo")]
        [TestCategory("API_SOAP_GetServiceInfo_Positive")]
        [TestMethod]
        public void BTA1476_ST1740_SOAP_GetServiceInfo_ValidateServiceInfo()
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
                stepName = "Get Organization, Environment, Loyalty Ware Assembly Version, Loyalty Ware Schema Version, CDIS Assembly Version and Validate";
                testStep = TestStepHelper.StartTestStep(testStep);
                GetServiceInfoOut services = cdis_Service_Method.GetServiceInfo(out elapsedTime);

                Assert.AreEqual(environment, services.Environment, "Expected environemnt is: "+ environment+" and Actual environment is: " +services.Environment);
                Assert.AreEqual(organization, services.Organization, "Expected environemnt is: " + organization + " and Actual environment is: " + services.Organization);
                Assert.AreEqual(loyaltyWareAssemblyVersion, services.LoyaltyWareAssemblyVersion, "Expected environemnt is: " + loyaltyWareAssemblyVersion + " and Actual environment is: " + services.LoyaltyWareAssemblyVersion);
                Assert.AreEqual(loyaltyWareSchemaVersion, services.LoyaltyWareSchemaVersion, "Expected environemnt is: " + loyaltyWareSchemaVersion + " and Actual environment is: " + services.LoyaltyWareSchemaVersion);
                Assert.AreEqual(cdisAssemblyVersion, services.CDISAssemblyVersion, "Expected environemnt is: " + cdisAssemblyVersion + " and Actual environment is: " + services.CDISAssemblyVersion);

                testStep.SetOutput("Service Info data is validated and Info is : " +
                    "Organization = " + services.Organization + ", " + 
                    "Environment = " +services.Environment+ ", " +
                    "Loyalty Ware Assembly Version = "+services.LoyaltyWareAssemblyVersion+ ", " +
                    "Loyalty Ware Schema version = "+services.LoyaltyWareSchemaVersion+ ", " +
                    "CDIS Assembly Version = "+services.CDISAssemblyVersion);
                Logger.Info("Service Info data is validated and Info is : " +
                    "Organization = " + services.Organization + ", " +
                    "Environment = " + services.Environment + ", " +
                    "Loyalty Ware Assembly Version = " + services.LoyaltyWareAssemblyVersion + ", " +
                    "Loyalty Ware Schema version = " + services.LoyaltyWareSchemaVersion + ", " +
                    "CDIS Assembly Version = " + services.CDISAssemblyVersion);

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
        [TestCategory("API_SOAP_GetServiceInfo")]
        [TestCategory("API_SOAP_GetServiceInfo_Positive")]
        [TestMethod]
        public void BTA1476_ST1741_SOAP_GetServiceInfo_ValidateElapsedTime()
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
                stepName = "Verify Elapsed Time of the output is positive non-zero value";
                testStep = TestStepHelper.StartTestStep(testStep);
                GetServiceInfoOut services = cdis_Service_Method.GetServiceInfo(out elapsedTime);
                Assert.AreEqual(elapsedTime>0 , true, "Expected Elapsed time should not be less than 0 where actual Elapsed time is : "+elapsedTime);
                testStep.SetOutput("Expected Elapsed time should be positive non-zero and actual Elapsed time is : "+elapsedTime);
                Logger.Info("Expected Elapsed time should be positive non-zero and actual Elapsed time is : " + elapsedTime);
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
        [TestCategory("API_SOAP_GetServiceInfo")]
        [TestCategory("API_SOAP_GetServiceInfo_Positive")]
        [TestMethod]
        public void BTA1476_ST1742_SOAP_GetServiceInfo_ValidateCDISAssemblyVersion()
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
                stepName = "Verify CDIS Assembly Version against Valid Endpoint";
                testStep = TestStepHelper.StartTestStep(testStep);
                GetServiceInfoOut services = cdis_Service_Method.GetServiceInfo(out elapsedTime);
                Assert.AreEqual(cdisAssemblyVersion, services.CDISAssemblyVersion, "Expected environemnt is: " + cdisAssemblyVersion + " and Actual environment is: " + services.CDISAssemblyVersion);
                testStep.SetOutput("Expected CDIS Assembly Version is : "+ cdisAssemblyVersion + " and Actual CDIS Assembly version is : "+services.CDISAssemblyVersion+ " against Endpoint: ");
                Logger.Info("Expected CDIS Assembly Version is : " + cdisAssemblyVersion + " and Actual CDIS Assembly version is : " + services.CDISAssemblyVersion + " against Endpoint: ");

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
        [TestCategory("API_SOAP_GetServiceInfo")]
        [TestCategory("API_SOAP_GetServiceInfo_Positive")]
        [TestMethod]
        public void BTA1476_ST1743_SOAP_GetServiceInfo_ValidateEnvironment()
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
                stepName = "Validate Environment against valid Endpoint";
                testStep = TestStepHelper.StartTestStep(testStep);
                GetServiceInfoOut services = cdis_Service_Method.GetServiceInfo(out elapsedTime);
                Assert.AreEqual(environment, services.Environment, "Expected environemnt is: " + environment + " and Actual environment is: " + services.Environment);
                testStep.SetOutput("Expected Environment is : " + environment + " and Actual Environment is : " + services.Environment + " against valid Endpoint");
                Logger.Info("Expected Environment is : " + environment + " and Actual Environment is : " + services.Environment + " against valid Endpoint");
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
        [TestCategory("API_SOAP_GetServiceInfo")]
        [TestCategory("API_SOAP_GetServiceInfo_Positive")]
        [TestMethod]
        public void BTA1476_ST1744_SOAP_GetServiceInfo_ValidateLoyaltyWareAssemblyVersion()
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
                stepName = "Validate Loyalty Ware Assembly version against valid Endpoint";
                testStep = TestStepHelper.StartTestStep(testStep);
                GetServiceInfoOut services = cdis_Service_Method.GetServiceInfo(out elapsedTime);
                Assert.AreEqual(loyaltyWareAssemblyVersion, services.LoyaltyWareAssemblyVersion, "Expected Loyalty Ware Assembly version is: " + loyaltyWareAssemblyVersion + " and Actual Loyalty Ware Assembly version is: " + services.LoyaltyWareAssemblyVersion);
                testStep.SetOutput("Expected Loyalty Ware Assembly version is : " + loyaltyWareAssemblyVersion + " and Actual Loyalty Ware Assembly version is : " + services.LoyaltyWareAssemblyVersion + " against valid Endpoint");
                Logger.Info("Expected Loyalty Ware Assembly version is : " + loyaltyWareAssemblyVersion + " and Actual Loyalty Ware Assembly version is : " + services.LoyaltyWareAssemblyVersion + " against valid Endpoint");
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
        [TestCategory("API_SOAP_GetServiceInfo")]
        [TestCategory("API_SOAP_GetServiceInfo_Positive")]
        [TestMethod]
        public void BTA1476_ST1745_SOAP_GetServiceInfo_ValidateLoyaltyWareSchemaVersion()
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
                stepName = "Validate Loyalty Ware Schema version against valid Endpoint";
                testStep = TestStepHelper.StartTestStep(testStep);
                GetServiceInfoOut services = cdis_Service_Method.GetServiceInfo(out elapsedTime);

                Assert.AreEqual(loyaltyWareSchemaVersion, services.LoyaltyWareSchemaVersion, "Expected Loyalty Ware Schema version is: " + loyaltyWareSchemaVersion + " and Actual Loyalty Ware Schema version is: " + services.LoyaltyWareSchemaVersion);

                testStep.SetOutput("Expected Loyalty Ware Schema version is : " + loyaltyWareSchemaVersion + " and Actual Loyalty Ware Schema version is : " + services.LoyaltyWareSchemaVersion + " against valid Endpoint");
                Logger.Info("Expected Loyalty Ware Schema version is : " + loyaltyWareSchemaVersion + " and Actual Loyalty Ware Schema version is : " + services.LoyaltyWareSchemaVersion + " against valid Endpoint");

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
        [TestCategory("API_SOAP_GetServiceInfo")]
        [TestCategory("API_SOAP_GetServiceInfo_Positive")]
        [TestMethod]
        public void BTA1476_ST1746_SOAP_GetServiceInfo_ValidateOrganization()
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
                stepName = "Validate Organization against valid Endpoint";
                testStep = TestStepHelper.StartTestStep(testStep);
                GetServiceInfoOut services = cdis_Service_Method.GetServiceInfo(out elapsedTime);

                Assert.AreEqual(organization, services.Organization, "Expected Organization is: " + organization + " and Actual Organization is: " + services.Organization);

                testStep.SetOutput("Expected Organization is : " + organization + " and Actual Organization is : " + services.Organization + " against valid Endpoint");
                Logger.Info("Expected Organization is : " + organization + " and Actual Organization is : " + services.Organization + " against valid Endpoint");

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