using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.Tests.IO.FileMethods;
using BnPBaseFramework.IO.BatFiles;
using TemplateItems;
using System.Text.RegularExpressions;
using System.IO;
//
namespace Bnp.Core.Tests.IO.IO_TestCases
{
    [TestClass]
    public class TestCases_TLogs : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        FileMethods_TLogs fileMethodsTlogs = new FileMethods_TLogs();

        [TestMethod]
        public void IO_CreateTLog_PositiveAssert()
        {

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG for positive assert";
                var filename = "TLog_PositiveAssert.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "Tlog_PositiveAssert");
                BatFile_Common.CreateBatchFile(JsonParser.TypeOfFile.Tlog);
                BatFile_Common.GenerateInputDataTextfile(JsonParser.TypeOfFile.Tlog, filename, ".txt");
                BatFile_Common.RunBatFile("TLOGFile");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                string pointsResult = tLogResults[0];
                string passedPointsCount = tLogResults[1];
                string failedPointsCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(pointsResult));
                Console.WriteLine("PassedCount is :{0}", passedPointsCount);
                Console.WriteLine("FailedCount is :{0}", failedPointsCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "PassedPointsCount: " + passedPointsCount + "FailedTLogCount: " + failedTLogCount + "FailedPointsCount: " + failedPointsCount);
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

        [TestMethod]
        public void IO_CreateTLog_NegativeAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG for negative assert";
                var filename = "TLog_NegativeAssert.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "TLog_NegativeAssert");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsFalse(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "FailedTLogCount: " + failedTLogCount);
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

        // [TestMethod]
        public void IO_CreateTLog_PositiveAndNegativeAssert()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG for positive and negative assert";
                var filename = "TLog_PositiveAndNegativeAssert.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "TLog_PositiveAndNegativeAssert");
                BatFile_Common.CreateBatchFile(JsonParser.TypeOfFile.Tlog);
                BatFile_Common.GenerateInputDataTextfile(JsonParser.TypeOfFile.Tlog, filename, ".txt");
                BatFile_Common.RunBatFile("TLOGFile");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "FailedTLogCount: " + failedTLogCount);
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


        [TestMethod]
        public void IO_CreateTLog_WithoutLoyaltyID()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG without LoyaltyID";
                var filename = "TLog_WithoutLoyaltyID.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "TLog_WithoutLoyaltyID");
                BatFile_Common.CreateBatchFile(JsonParser.TypeOfFile.Tlog);
                BatFile_Common.GenerateInputDataTextfile(JsonParser.TypeOfFile.Tlog, filename, ".txt");
                BatFile_Common.RunBatFile("TLOGFile");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "FailedTLogCount: " + failedTLogCount);
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


        [TestMethod]
        public void IO_CreateTLog_WithoutTxnID()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG without TxnID";
                var filename = "TLog_WithoutTxnID.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "TLog_WithoutTxnID");
                BatFile_Common.CreateBatchFile(JsonParser.TypeOfFile.Tlog);
                BatFile_Common.GenerateInputDataTextfile(JsonParser.TypeOfFile.Tlog, filename, ".txt");
                BatFile_Common.RunBatFile("TLOGFile");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "FailedTLogCount: " + failedTLogCount);
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


        [TestMethod]
        public void IO_CreateTLog_WithoutRegisterID()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG without RegisterID";
                var filename = "TLog_WithoutRegisterID.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "TLog_WithoutRegisterID");
                BatFile_Common.CreateBatchFile(JsonParser.TypeOfFile.Tlog);
                BatFile_Common.GenerateInputDataTextfile(JsonParser.TypeOfFile.Tlog, filename, ".txt");
                BatFile_Common.RunBatFile("TLOGFile");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "FailedTLogCount: " + failedTLogCount);
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


        [TestMethod]
        public void IO_CreateTLog_WithoutTotalTxnAmt()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG without TotalTxnAmt";
                var filename = "TLog_WithoutTotalTxnAmt.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "TLog_WithoutTotalTxnAmt");
                BatFile_Common.CreateBatchFile(JsonParser.TypeOfFile.Tlog);
                BatFile_Common.GenerateInputDataTextfile(JsonParser.TypeOfFile.Tlog, filename, ".txt");
                BatFile_Common.RunBatFile("TLOGFile");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "FailedTLogCount: " + failedTLogCount);
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


        [TestMethod]
        public void IO_CreateTLog_WithoutTxnDetailID()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Creating TLOG without TxnDetailId";
                var filename = "TLog_WithoutTxnDetailID.txt";
                JsonParser.JsonFileProcessor.GenerateInputData(JsonParser.TypeOfFile.Tlog, "TLog_WithoutTxnDetailID");
                BatFile_Common.CreateBatchFile(JsonParser.TypeOfFile.Tlog);
                BatFile_Common.GenerateInputDataTextfile(JsonParser.TypeOfFile.Tlog, filename, ".txt");
                BatFile_Common.RunBatFile("TLOGFile");
                string[] tLogResults = fileMethodsTlogs.WriteToFile(filename);
                string tLogResult = tLogResults[0];
                string passedtLogCount = tLogResults[1];
                string failedTLogCount = tLogResults[2];
                Assert.IsTrue(Convert.ToBoolean(tLogResult));
                Console.WriteLine("PassedCount is :{0}", passedtLogCount);
                Console.WriteLine("FailedCount is :{0}", failedTLogCount);
                testStep.SetOutput("PassedTLogCount: " + passedtLogCount + "FailedTLogCount: " + failedTLogCount);
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

            // tLogResults = null;
            FileMethods_TLogs.lineNum = 0;
            //FileMethods_TLogs.
            // FileMethods_TLogs.Oracleresults = string.Empty;
            // FileMethods_TLogs.result = false;


        }

    }
}
