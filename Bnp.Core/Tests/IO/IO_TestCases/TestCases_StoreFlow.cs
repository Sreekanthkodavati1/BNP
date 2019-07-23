using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.Tests.IO.FileMethods;

namespace Bnp.Core.Tests.IO.IO_TestCases
{
    /// <summary>
    /// Tests to test Store File creation with different input file templates.
    /// </summary>
    [TestClass]
    public class TestCases_StoreFlow : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// Tests to test Store File creation with all the mandatory attributes having NOT NULL values in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_PositiveAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store for positive assert";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Stores);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_AllAttributesPositive.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_AllAttributesPositive.txt");
                Console.WriteLine(expected);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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



        /// <summary>
        /// Tests to test Store File creation with the mandatory attribute "PhoneNumber" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_WithoutPhoneNumber()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store without phone number";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Stores);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_WithoutPhoneNumber.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_WithoutPhoneNumber.txt");
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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



        /// <summary>
        /// Tests to test Store File creation with the mandatory attribute "AddressLineOne" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_WithoutAddressLineOne()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store without address line one";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Stores);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_WithoutAddressLineOne.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_WithoutAddressLineOne.txt");

                Console.WriteLine(expected);
                Console.WriteLine(actual);

                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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


        /// <summary>
        /// Tests to test Store File creation with the mandatory attribute "City" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_WithoutCity()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store without city";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Stores);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_WithoutCity.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_WithoutCity.txt");

                Console.WriteLine(expected);
                Console.WriteLine(actual);

                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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


        /// <summary>
        /// Tests to test Store File creation with the mandatory attribute "Country" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_WithoutCountry()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store without country";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Stores);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_WithoutCountry.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_WithoutCountry.txt");
                Console.WriteLine(expected);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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


        /// <summary>
        /// Tests to test Store File creation with the mandatory attribute "StateOrProvince" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_WithoutStateOrProvince()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store without stateorprovince";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Stores);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_WithoutState.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_WithoutState.txt");
                Console.WriteLine(expected);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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


        /// <summary>
        /// Tests to test Store File creation with the combination of  mandatory and non mandatory attributes having NULL and NOT NULL values respectively in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_PositiveAndNegativeAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store for positive and negative flow";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Store);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_PositiveAndNegativeAssert.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_PositiveAndNegativeAssert.txt");

                Console.WriteLine(expected);
                Console.WriteLine(actual);

                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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


        /// <summary>
        /// Tests to test Store File creation when the result set in PLSQL DB(local) doesnt match with the values in Oracle DB(client) for a input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateStore_NegativeAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating store for Negative assert";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Stores);
                string actual = FileMethods_Store.VerifyMandatoryFields("Store_Negativeassert.txt");
                string expected = FileMethods_Store.ReadMandatoryValuesFrmPLSQL("Store_NegativeAssert.txt");
                Console.WriteLine(expected);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                testStep.SetOutput("Expected: " + expected + " Actual: " + actual);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("IO"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("IO"));
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

        [TestCleanup]
        public void CleanUp()
        {
            FileMethods_Store.PLSQLresults = string.Empty;
            FileMethods_Store.Oracleresults = string.Empty;
        }



    }
}
