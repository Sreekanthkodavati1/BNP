using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.Tests.IO.FileMethods;

namespace Bnp.Core.Tests.IO.IO_TestCases
{
    /// <summary>
    /// Tests to test Enrollment file creation with different input file templates.
    /// </summary>
    [TestClass]
    public class TestCases_Enrollment : ProjectTestBase
    {

        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;


        /// <summary>
        /// Tests to test Enrollment File creation with all the mandatory attributes having NOT NULL values in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateEnrollment_PositiveAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating enrollment for positive assert";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Enrollment);
                string actual = FileMethods_Enrollment.VerifyMandatoryFields("Enrollment_PositiveAssert.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Enrollment.ReadMandatoryValuesFrmPLSQL("Enrollment_PositiveAssert.txt");
                Console.WriteLine(actual);
                Console.WriteLine(expected);
                Assert.AreEqual(expected, actual);
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
        /// Tests to test Enrollment File creation with the combination of  mandatory and non mandatory attributes having NULL and NOT NULL values respectively in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateEnrollment_PositiveAndNegativeAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating enrollment for a combination of positive and Negative flow";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Enrollment);
                string actual = FileMethods_Enrollment.VerifyMandatoryFields("Enrollment_PositiveAndNegativeAssert.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Enrollment.ReadMandatoryValuesFrmPLSQL("Enrollment_PositiveAndNegativeAssert.txt");
                Console.WriteLine(actual);
                Console.WriteLine(expected);
                Assert.AreEqual(expected, actual);
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
        /// Tests to test Enrollment File creation with the mandatory attribute "LoyaltyId" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateEnrollment_WithoutLoyaltyId()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating enrollment without loyaltyId";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Enrollment);
                string actual = FileMethods_Enrollment.VerifyMandatoryFields("Enrollment_WithoutLoyaltyId.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Enrollment.ReadMandatoryValuesFrmPLSQL("Enrollment_WithoutLoyaltyId.txt");
                Console.WriteLine(actual);
                Console.WriteLine(expected);
                Assert.AreEqual(expected, actual);
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
        /// Tests to test Enrollment File creation with the mandatory attribute "AlternateId" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateEnrollment_WithoutAlternateId()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating enrollment without AlternateId";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Enrollment);
                string actual = FileMethods_Enrollment.VerifyMandatoryFields("Enrollment_WithoutAlternateId.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Enrollment.ReadMandatoryValuesFrmPLSQL("Enrollment_WithoutAlternateId.txt");
                Console.WriteLine(actual);
                Console.WriteLine(expected);
                Assert.AreEqual(expected, actual);
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
        /// Tests to test Enrollment File creation with the mandatory attribute "UserName" having NULL value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateEnrollment_WithoutUserName()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating enrollment without UserName";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Enrollment);
                string actual = FileMethods_Enrollment.VerifyMandatoryFields("Enrollment_WithoutUserName.txt");
                //StoreFunctions.RunStoreBatFile();
                string expected = FileMethods_Enrollment.ReadMandatoryValuesFrmPLSQL("Enrollment_WithoutUserName.txt");
                Console.WriteLine(actual);
                Console.WriteLine(expected);
                Assert.AreEqual(expected, actual);
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
        /// Tests to test Enrollment File creation when the result set in PLSQL DB(local) doesnt match with the values in Oracle DB(client) for a input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateEnrollment_NegativeAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating enrollment for negative assert";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Enrollment);
                string actual = FileMethods_Enrollment.VerifyMandatoryFields("Enrollment_NegativeAssert.txt");
                string expected = FileMethods_Enrollment.ReadMandatoryValuesFrmPLSQL("Enrollment_NegativeAssert.txt");
                Console.WriteLine(actual);
                Console.WriteLine(expected);
                Assert.AreNotEqual(expected, actual);
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
        /// Tests to test Enrollment File creation with existing primaryEmailAddress value in the input file template.
        /// </summary>
        [TestMethod]
        public void IO_CreateEnrollmentWithExistingEmail()
        {

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating enrollment with existing email assert";
                //JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Enrollment);
                string actual = FileMethods_Enrollment.VerifyMandatoryFields("Enrollment_WithExistingEmail.txt");
                //StoreFunctions.RunStoreBatFile();
                String expected = FileMethods_Enrollment.ReadMandatoryValuesFrmPLSQL("Enrollment_WithExistingEmail.txt");
                Console.WriteLine(actual);
                Console.WriteLine(expected);
                Assert.AreNotEqual(expected, actual);
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
            FileMethods_Enrollment.PLSQLresults = string.Empty;
            FileMethods_Enrollment.Oracleresults = string.Empty;

        }
    }
}




