using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BnPBaseFramework.Web;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using System;
using System.Configuration;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.Database;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA82_Navigator_GenerateDBconfig_AddinConfigfolder : ProjectTestBase
    {
        public string OrderTest_Status = "";

        public BTA82_Navigator_GenerateDBconfig_AddinConfigfolder()
        {
        }
        public BTA82_Navigator_GenerateDBconfig_AddinConfigfolder(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        Login login = new Login();
        DB db = new DB();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;


        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA82_Navigator_GenerateDBConfig()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testStep = new TestStep();
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);


            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string StepOutput = "";
            bool stepstatus;
            try
            {
                #region:OrderExection Requirement
                string Prereq_testCase = "BTA79_Navigator_CreateDBAAdminUser_And_Login_With_DBAAdminUser";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase, methodName, testStep);
                #endregion 

                #region Step1:Verify Key Files Existed ,If Existed Delete Existed Files
                stepName = "Delete Existed DBConfig.dat File From :" + basePages.ConfigDownloadPath;
                testStep = TestStepHelper.StartTestStep(testStep);

                string dbFile = basePages.ConfigDownloadPath + @"\DBConfig.dat";
                basePages.DeleteExistedFile(dbFile);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);

                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Login As DB Admin User 
                stepName = "Login As DB Admin User and Navigate to Keys Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.DBAUser;
                login.Password = NavigatorUsers.DBAUser_Password;
                navigator_LoginPage.Login(login, Users.AdminRole.DBA.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_DBUser_HomePage = new Navigator_DBHomePage(DriverContext);
                navigator_DBUser_HomePage.NavigatetoDatabases_Page(out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select Organization and Environment 
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_DBDevHomePage = new Navigator_DBFramework_Page(DriverContext);
                string Org_Output; string Env_Output;
                navigator_DBDevHomePage.DrillDownOrg(out Org_Output);
                navigator_DBDevHomePage.SelectEnvironment(out Env_Output);
                testStep.SetOutput(Org_Output + ";" + Env_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Creating DB Connection is not existed ,Generate DBConfig
                stepName = "Creating DB Connection";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_DBDevHomePage.CreatingDBConnection(db.Name, db.DataBasesType, db.UserID, db.Password, db.DefaultSchema, db.Server, db.Database, db.ConnectionProps, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Generate DBConfig file and Download
                stepName = "Generate DBConfig file and Download";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = navigator_DBDevHomePage.Download_DBConfigFile(dbFile, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Create Config Folder for Organization if not existed
                stepName = "Create Config Folder for Organization if not existed";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.CreateOrVerfiyConfigFolder();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Copy DB file
                stepName = "Copy DBConfig file to Config folder";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.CopyFile(dbFile, basePages.ConfigUploadPath + @"\DBConfig.dat");
                stepstatus = basePages.VerifyExistedorDownloadedFile(basePages.ConfigUploadPath + @"\DBConfig.dat", "DBConfig.datFile uploaded Successfully in config path:" + basePages.ConfigUploadPath, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Initialize DB Framework if the DB is not Initialized
                stepName = "Initialize DB Framework if the DB is not Initialized";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = navigator_DBDevHomePage.InitializeFrameworkDB(true, true, out StepOutput); testStep.SetOutput(StepOutput);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: LogOut
                testStep = TestStepHelper.StartTestStep(testStep);
                if (StepOutput.Contains("New DB Initialization is Completed"))
                {
                    stepName = "Verfiying intialization";

                    //navigator_LoginPage.Logout();
                    //navigator_LoginPage.Login(login, Users.AdminRole.DBA.ToString(), out stroutput);
                    navigator_DBUser_HomePage.NavigatetoDatabases_Page(out StepOutput);
                    navigator_DBDevHomePage.DrillDownOrg(out Org_Output);
                    navigator_DBDevHomePage.SelectEnvironment(out Env_Output);
                    stepstatus = navigator_DBDevHomePage.InitializeFrameworkDB(true, true, out StepOutput); testStep.SetOutput(StepOutput);
                }

                stepName = "Logout User";
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
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
