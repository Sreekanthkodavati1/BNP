using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using System;
using System.Configuration;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.Keys;
using Bnp.Core.WebPages.Navigator.Database;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA81_Navigator_GenerateKeys_AddinConfigfolder : ProjectTestBase
    {
        public string OrderTest_Status = "";

        public BTA81_Navigator_GenerateKeys_AddinConfigfolder()
        {
        }
        public BTA81_Navigator_GenerateKeys_AddinConfigfolder(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA81_Navigator_GenerateKeys()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testStep = new TestStep();
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);

            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string stepOutput = "";
            try
            {
                #region:OrderExection Requirement
                string Prereq_testCase = "BTA79_Navigator_CreateKEYAdminUser_And_Login_With_KEYAdminUser";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase, methodName, testStep);
                #endregion 

                #region Step1:Verify Key Files Existed ,If Existed Delete Existed Files
                stepName = "Delete Existed Keystore.dat Files  and SymmetricKeystore.dat from :" + basePages.ConfigDownloadPath;
                testStep = TestStepHelper.StartTestStep(testStep);

                string keyStorefile = basePages.ConfigDownloadPath + @"\Keystore.dat";
                string symmetricKeystore = basePages.ConfigDownloadPath + @"\SymmetricKeystore.dat";
                basePages.DeleteExistedFile(keyStorefile);
                basePages.DeleteExistedFile(symmetricKeystore);
                stepOutput = "Existed Keystore.dat Files  and SymmetricKeystore.dat from :" + basePages.ConfigDownloadPath + " Deleted Successfully"; testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);

                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Login As Key Admin User 
                stepName = "Login As Key Admin User and Navigate to Keys Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_KeyUser_HomePage = new Navigator_KeysHomePage(DriverContext);
                var navigator_MangeKeysPage = new Navigator_ManageKeysPage(DriverContext);
                login.UserName = NavigatorUsers.KEYUser;
                login.Password = NavigatorUsers.KEYUser_Password;
                navigator_LoginPage.Login(login, Users.AdminRole.KEY.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                navigator_KeyUser_HomePage.NavigatetoMangeKeys_Page(out stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select Organization and Environment 
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_DBDevHomePage = new Navigator_DBFramework_Page(DriverContext);
                string Org_Output; string Env_Output;
                navigator_DBDevHomePage.DrillDownOrg(out Org_Output);
                navigator_DBDevHomePage.SelectEnvironment(out Env_Output); testStep.SetOutput(Org_Output + ";" + Env_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Generate Keys file Symmetric Key file with valid Password details and Export
                stepName = "Generate Keys file  Symmetric Key file with valid Password details and Export";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_MangeKeysPage.EnterKeyStorePassword("Password1*", "512", out stepOutput); string Output1 = stepOutput;
                navigator_MangeKeysPage.ExportPrivateKey(keyStorefile, out stepOutput); string Output2 = stepOutput;
                navigator_MangeKeysPage.ExportSymmetrickey(symmetricKeystore, out stepOutput); string Output3 = stepOutput; testStep.SetOutput(";" + Output1 + ".;" + Output2 + ".;" + Output3);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Create Config Folder for Organization if not existed
                stepName = "Create Config Folder for Organization if not existed";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.CreateOrVerfiyConfigFolder();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Copy Keystore file
                stepName = "Copy Keystore file to Config folder";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.CopyFile(keyStorefile, basePages.ConfigUploadPath + @"\Keystore.dat");
                basePages.VerifyExistedorDownloadedFile(basePages.ConfigUploadPath + @"\Keystore.dat", "Keystore.dat File uploaded Successfully to config path:" + basePages.ConfigUploadPath, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Copy SymmetricKeystore file
                stepName = "Copy SymmetricKeystore file to Config folder";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.CopyFile(symmetricKeystore, basePages.ConfigUploadPath + @"\SymmetricKeystore.dat");
                basePages.VerifyExistedorDownloadedFile(basePages.ConfigUploadPath + @"\SymmetricKeystore.dat", "SymmetricKeystore.dat File uploaded Successfully to config path:" + basePages.ConfigUploadPath, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: LogOut
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
                testStep.SetOutput(stepName + e);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
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
