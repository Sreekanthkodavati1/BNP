using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Logger;
using OpenQA.Selenium;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Reports;
using System.IO;
using BnPBaseFramework.Reporting.Utils;
using System.Configuration;
using BnPBaseFramework.Reporting.DAL;
using System.Diagnostics;
using System.Linq;
using Brierley.LoyaltyWare.ClientLib;
using System.Reflection;
using Bnp.Core.Tests.API.Validators;
using System.Data;
using BnPBaseFramework.Reporting.Jira;

namespace Bnp.Core
{
    [TestClass]
    public class ProjectTestBase : TestBase
    {
        public DriverContext driverContext;
        public static DriverContext dr;
        public static TestSuite testSuite = null;
        public static TestSuite testJiraSuite = null;
        public static List<TestCase> listOfTestCases = null;
        public static List<TestStep> listOfTestCaseSteps;
        public static LWIntegrationSvcClientManager lWIntegrationSvcClientManager = null;
        public string[] customReportMetadata;
        public static DataTable TEST_DATA_FOR_AUTH_TOKEN = null;
        
        public ProjectTestBase()
        {
        }

        public ProjectTestBase(DriverContext driverContext)
        {
            DriverContext = driverContext;
            Driver = driverContext.Driver;
        }

        protected IWebDriver Driver { get; set; }

        /// <summary>
        /// Gets or sets logger instance for driver
        /// </summary>
        public TestLogger LogTest
        {
            get
            {
                return DriverContext.LogTest;
            }

            set
            {
                DriverContext.LogTest = value;
            }
        }

        /// <summary>
        /// Gets or sets the microsoft test context.
        /// </summary>
        /// <value>
        /// The microsoft test context.
        /// </value>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Gets the driver context
        /// </summary>
        protected DriverContext DriverContext
        {
            set { driverContext = value; }
            get
            {
                return driverContext;
            }
        }
        /// <summary>
        /// Before the assembley intialization.
        /// </summary>
        [AssemblyInitialize]
        public static void AssemblyInitialization(TestContext TestContext)
        {
            testSuite = new TestSuite();
            testJiraSuite = testSuite;
            testSuite.SetSuiteStartTime(new StringHelper().GetFormattedDateTimeNow());
            testJiraSuite.SetSuiteStartTime(new StringHelper().GetFormattedDateTimeNow("jira"));
            Console.WriteLine("Suite start time = " + testSuite.GetSuiteStartTime());
            listOfTestCases = new List<TestCase>();
            lWIntegrationSvcClientManager = new LWIntegrationSvcClientManager(ConfigurationManager.AppSettings["SoapServiceURL"], "CDIS", true, String.Empty);
            TEST_DATA_FOR_AUTH_TOKEN = DatabaseUtility.GetTestDataForAuthTokenTask();
        }

        /// <summary>
        /// Before the test.
        /// </summary>
        [TestInitialize]
        public void BeforeTest()
        {
            driverContext = new DriverContext();
            dr = driverContext;
            customReportMetadata = ReportDetails(TestContext.FullyQualifiedTestClassName);
            testSuite.ApplicationName = customReportMetadata[4];
            testSuite.environmentName = customReportMetadata[0];
            testSuite.ClientID = customReportMetadata[3];
            testSuite.SuiteName = customReportMetadata[1];
            testSuite.buildVersion = customReportMetadata[2];
            DriverContext.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DriverContext.TestTitle = TestContext.TestName;
            LogTest.LogTestStarting(driverContext);
            if ((!TestContext.FullyQualifiedTestClassName.ToString().Contains("API")) & (!TestContext.FullyQualifiedTestClassName.ToString().Contains("IO")) & (!OrderExecutionTests.ordTestSuiteName.Contains("API")))
            {
                DriverContext.Start();
            }
        }

        /// <summary>
        /// After the test.
        /// </summary>
        [TestCleanup]
        public void AfterTest()
        {
            DriverContext.IsTestFailed = TestContext.CurrentTestOutcome == UnitTestOutcome.Failed || !driverContext.VerifyMessages.Count.Equals(0);
            var filePaths = SaveTestDetailsIfTestFailed(driverContext, TestContext.FullyQualifiedTestClassName);
            DriverContext.Stop();            
            LogTest.LogTestEnding(driverContext);
        }

        /// <summary>
        /// Assembley cleanup.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            string IsDBReportingRequired = ConfigurationManager.AppSettings["IsDBReportingRequired"];
            string IsXRayIntegrationRequired = ConfigurationManager.AppSettings["IsXRayIntegrationRequired"];
            DataAccessLayer dall = new DataAccessLayer();
            XRay xRay = new XRay();
            testSuite.SetSuiteEndTime(new StringHelper().GetFormattedDateTimeNow());
            testJiraSuite.SetSuiteEndTime(new StringHelper().GetFormattedDateTimeNow("jira"));
            Console.WriteLine("Suite end time = " + testSuite.GetSuiteEndTime());
       //     testSuite.SetSuiteName("BnP End to End Test");
            testSuite.SetTotalTestCasesExecuted(listOfTestCases.Count);
            int passedTestCases = 0;

            if (IsDBReportingRequired == "true")
            {
                foreach (TestCase testCase in listOfTestCases)
                {
                    if (testCase.IsStatus())
                    {
                        passedTestCases++;
                    }
                    if (listOfTestCases.Count > 0)
                    {
                        List<TestCase> TestCaseList = listOfTestCases;
                        foreach (TestCase testCases in TestCaseList)
                        {
                            testCases.SetTestSuiteId(testSuite.GetId());
                            if (testCases.IsStatus())
                            {
                                testCases.Setdbstatus(1);
                            }
                            else
                            {
                                testCases.Setdbstatus(0);
                            }
                            dall.CreateTestCase(testCase);
                            if (testCases.GetId() != null)
                            {
                                var id = testCases.GetId();
                                List<TestStep> TestStepLists = testCases.GetTestCaseSteps();
                                foreach (TestStep TS in TestStepLists)
                                {
                                    TS.SetTestCaseid(id);
                                }
                            }
                            if (testCases.GetTestCaseSteps() != null)
                            {
                                List<TestStep> TestStepList = testCases.GetTestCaseSteps();
                                foreach (TestStep teststep in TestStepList)
                                {
                                    if (teststep.IsStatus())
                                    {
                                        teststep.SetTestStepdbstatus(1);
                                    }
                                    else
                                    {
                                        teststep.SetTestStepdbstatus(0);
                                    }
                                    dall.CreateTestStep(teststep);
                                }
                            }
                        }
                    }
                }
                testSuite.SetPassed(passedTestCases);
                testSuite.SetFailed((listOfTestCases.Count - passedTestCases));
                testSuite.SetListOfTestCases(listOfTestCases);
                dall.Create(testSuite);
                testSuite = dall.GetTestSuit(testSuite);
                if(IsXRayIntegrationRequired.ToLower() == "true")
                    xRay.PostJiraResults(testSuite, testJiraSuite);
                new ProjectTestBase().GenerateHTMLReport(testSuite);
                }
            else
            {
                foreach (TestCase testCase in listOfTestCases)
                {
                    if (testCase.IsStatus())
                    {
                        passedTestCases++;
                    }
                }
                testSuite.SetPassed(passedTestCases);
                testSuite.SetFailed((listOfTestCases.Count - passedTestCases));
                testSuite.SetListOfTestCases(listOfTestCases);
                if (IsXRayIntegrationRequired.ToLower() == "true")
                    xRay.PostJiraResults(testSuite, testJiraSuite);
                new ProjectTestBase().GenerateHTMLReport(testSuite);
                
                
            }
            lWIntegrationSvcClientManager.Dispose();
            //Killing IE, chrome and gecko driver server process if any present during Assembly cleanup
            Process.GetProcessesByName("IEDriverServer").ToList().ForEach(p => p.Kill());
            Process.GetProcessesByName("chromedriver").ToList().ForEach(p => p.Kill());
            Process.GetProcessesByName("geckodriver").ToList().ForEach(p => p.Kill());
        }

        private void SaveAttachmentsToTestContext(string[] filePaths)
        {
            if (filePaths != null)
            {
                foreach (var filePath in filePaths)
                {
                    LogTest.Info("Uploading file [{0}] to test context", filePath);
                    TestContext.AddResultFile(filePath);
                }
            }
        }

        private void GenerateHTMLReport(TestSuite testSuite)
        {
            HtmlReportGenerator htmlReportGenerator = new HtmlReportGenerator();
            string currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string str = DateTime.Now.ToString("yyyyMMddHHmmss");
            StreamWriter writer;
            if (ConfigurationManager.AppSettings["JenkinsExecution"].Equals("true"))
            {
                string jenkinsBasePath = System.Environment.GetEnvironmentVariable("Workspace");
                // string jenkinsBasePath = "C:\\Program Files (x86)\\Jenkins\\workspace\\Bnp_52012";
                writer = new StreamWriter(jenkinsBasePath + "\\Bnp.Core\\Results\\BnP_TestReport.html");
            }
            else
            {
                writer = new StreamWriter(currentDirectoryPath.Substring(0, currentDirectoryPath.IndexOf("BnPAutomation")) + "BnPAutomation\\Bnp.Core\\Results\\BnP_TestReport_" + str + ".html");
            }
            writer.Write(htmlReportGenerator.GenerateHTMLReport(testSuite));
            writer.Flush();
            writer.Close();
        }

        public string [] ReportDetails(string aName)
        {
            string Environment = string.Empty;
            string testSuiteName = string.Empty;
            string buildVersion = string.Empty;
            string clientID = string.Empty;
            string appUnderTest = string.Empty;
            ClientID clientIdObj;
            Type clientIdClassType;
            string[] appName = aName.Split('.');

            if (ConfigurationManager.AppSettings["JenkinsExecution"].Equals("true"))
            {
                 Environment = System.Environment.GetEnvironmentVariable("Env");
                 testSuite.SetSuiteName(System.Environment.GetEnvironmentVariable("ExecutionType"));
                 //testSuiteName = System.Environment.GetEnvironmentVariable("ExecutionType");
                 buildVersion = System.Environment.GetEnvironmentVariable("Build_Version");
                if (aName.StartsWith("Bnp.Core"))
                {
                    clientIdObj = new ClientID();
                    clientIdClassType = clientIdObj.GetType();
                    clientID = clientIdClassType.GetField("Core").GetValue(clientIdObj).ToString();
                }
                else
                {
                    clientIdObj = new ClientID();
                    clientIdClassType = clientIdObj.GetType();
                    clientID = clientIdClassType.GetField(appName[0]).GetValue(clientIdObj).ToString();
                }
                appUnderTest = " " + appName[3].ToString() + " - " + appName[4].ToString();
                return new[] { Environment, testSuiteName, buildVersion, clientID, appUnderTest};
            }
            else if (aName.Equals("Bnp.Core.OrderExecutionTests"))
            {
                Environment = ConfigurationManager.AppSettings["Environment"];
                // testSuiteName = OrderExecutionTests.ordTestSuiteName;
                testSuite.SetSuiteName(OrderExecutionTests.ordTestSuiteName);
                buildVersion = ConfigurationManager.AppSettings["Version"];
                if (aName.StartsWith("Bnp.Core"))
                {
                    clientIdObj = new ClientID();
                    clientIdClassType = clientIdObj.GetType();
                    clientID = clientIdClassType.GetField("Core").GetValue(clientIdObj).ToString();
                }
                else
                {
                    clientIdObj = new ClientID();
                    clientIdClassType = clientIdObj.GetType();
                    clientID = clientIdClassType.GetField(appName[0]).GetValue(clientIdObj).ToString();
                }
                appUnderTest = "";
                //appUnderTest = " " + appName[3].ToString() + " - " + appName[4].ToString();
                return new[] { Environment, testSuiteName, buildVersion, clientID, appUnderTest };
            }
            else
            {
                Environment = ConfigurationManager.AppSettings["Environment"];
                //testSuiteName = ConfigurationManager.AppSettings["TestSuiteName"];
                testSuite.SetSuiteName(ConfigurationManager.AppSettings["TestSuiteName"]);
                buildVersion = ConfigurationManager.AppSettings["Version"];
                if (aName.StartsWith("Bnp.Core"))
                {
                    clientIdObj = new ClientID();
                    clientIdClassType = clientIdObj.GetType();
                    clientID = clientIdClassType.GetField("Core").GetValue(clientIdObj).ToString();
                }
                else
                {
                    clientIdObj = new ClientID();
                    clientIdClassType = clientIdObj.GetType();
                    clientID = clientIdClassType.GetField(appName[0]).GetValue(clientIdObj).ToString();
                }
                appUnderTest = " " + appName[3].ToString() + " - " + appName[4].ToString();
                return new[] { Environment, testSuiteName, buildVersion, clientID, appUnderTest };
            }
        }

    }
}
