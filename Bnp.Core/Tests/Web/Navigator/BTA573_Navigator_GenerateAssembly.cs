using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA573_Navigator_GenerateAssembly_clientAssembly : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        public string OrderTest_Status = "";

        public BTA573_Navigator_GenerateAssembly_clientAssembly()
        {
        }
        public BTA573_Navigator_GenerateAssembly_clientAssembly(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA573_Navigator_GenerateAssembly()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string stroutput = "";
            try
            {
                #region Pre-Conditions
                Application_Nav_Util_Page Model = new Application_Nav_Util_Page(DriverContext);
                Navigator_GenerateAssembly nav_GenerateAssembly = new Navigator_GenerateAssembly(DriverContext);
                string assemblyDev = basePages.ConfigDownloadPath + @"\" + ProjectBasePage.Orgnization_value + ".zip";
                string assemblyXsd = basePages.ConfigDownloadPath + @"\XSD.zip";
                string assemblyJava = basePages.ConfigDownloadPath + @"\LWIntgr-Java.zip";
                string assemblyDotnet = basePages.ConfigDownloadPath + @"\LWIntgr-DotNet.zip";
                basePages.DeleteExistedFile(assemblyDev);
                basePages.DeleteExistedFile(assemblyXsd);
                basePages.DeleteExistedFile(assemblyJava);
                basePages.DeleteExistedFile(assemblyDotnet);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Generate Assembly
                stepName = "Generate Assembly in Navigator";
                testStep = TestStepHelper.StartTestStep(testStep);
                Model.OpenApplication(NavigatorEnums.ApplicationName.model);
                nav_GenerateAssembly.GenerateAssembly(out stroutput);
                testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Generate dot net client Assembly
                stepName = "Generate dot net client Assembly in Navigator";
                testStep = TestStepHelper.StartTestStep(testStep);
                Model.OpenApplication(NavigatorEnums.ApplicationName.model);
                nav_GenerateAssembly.GenerateDotNetClientAssembly(out stroutput);
                testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Generate java client Assembly
                stepName = "Generate java client Assembly in Navigator";
                testStep = TestStepHelper.StartTestStep(testStep);
                Model.OpenApplication(NavigatorEnums.ApplicationName.model);
                nav_GenerateAssembly.GenerateJavaClientAssembly(out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Generate XSD client Assembly
                stepName = "Generate XSD client Assembly in Navigator";
                testStep = TestStepHelper.StartTestStep(testStep);
                Model.OpenApplication(NavigatorEnums.ApplicationName.model);
                nav_GenerateAssembly.GenerateXSDClientAssembly(out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify Assembly downloaded
                stepName = "Verify Dev Assembly downloaded";
                testStep = TestStepHelper.StartTestStep(testStep);
                Thread.Sleep(3000);//This is exclusive to wait for the download time
                basePages.VerifyExistedorDownloadedFile(assemblyDev, "BTADEV File downloaded Successfully to config path:" + basePages.ConfigDownloadPath, out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Verify DotNet Assembly downloaded
                stepName = "Verify DotNet Assembly downloaded";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.VerifyExistedorDownloadedFile(assemblyDotnet, "DotNet File downloaded Successfully to config path:" + basePages.ConfigDownloadPath, out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Verify Java Assembly downloaded
                stepName = "Verify Java Assembly downloaded";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.VerifyExistedorDownloadedFile(assemblyJava, "Java File downloaded Successfully to config path:" + basePages.ConfigDownloadPath, out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Verify XSD Assembly downloaded
                stepName = "Verify XSD Assembly downloaded";
                testStep = TestStepHelper.StartTestStep(testStep);
                Thread.Sleep(10000);
                basePages.VerifyExistedorDownloadedFile(assemblyXsd, "XSD File downloaded Successfully to config path:" + basePages.ConfigDownloadPath, out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: LogOut
                stepName = "Logout as USER Admin With All roles";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Passed");

            }

            catch (Exception e)
            {
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Failed");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                if (!OrderTest_Status.Contains("true"))
                {
                    Assert.Fail(); testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                }
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
