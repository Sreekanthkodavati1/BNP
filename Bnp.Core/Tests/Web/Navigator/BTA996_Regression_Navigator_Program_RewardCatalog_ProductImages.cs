using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.RewardCatalog;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// Test to test BTA-996  : Create, Edit and Delete Product Image
    /// </summary>
    [TestClass]
    public class BTA996_Regression_Navigator_Program_RewardCatalog_ProductImages : ProjectTestBase
    {
        Login login = new Login();
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        readonly string password = NavigatorUsers.NavigatorPassword;

        /// <summary>
        /// Test BTA_97 : Create a Product Image
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_97_LN_CreateProductImage()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            string imageName = WebsiteData.ProductImageName + RandomDataHelper.RandomString(5);
            string imageOrder = WebsiteData.ProductImageOrder;
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Click on reward catalog and verify product images tab displayed 
                stepName = "Click on reward catalog and verify product images tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product images tab and verify product images panel displayed 
                stepName = "Click on product images tab and verify product images panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductImages);
                testStep.SetOutput("Product images panel Openedsuccessfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify the existence of category and create category if doesn't exist
                stepName = "Verify the existence of category and create category if doesn't exist";
                testStep = TestStepHelper.StartTestStep(testStep);
                rewardCatlog_productImagePage.VerifyCategoryExistanceAndCreateIfNotExists(product, out string outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                
                #region Step7: Verify the existence of product and create product if doesn't exist
                stepName = "Verify the existence of product and create product if doesn't exist";
                testStep = TestStepHelper.StartTestStep(testStep);
                rewardCatlog_productImagePage.VerifyProductExistanceAndCreateIfNotExists(product, out outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Click on create new product image button and verify new product image panel opened successfully
                stepName = "Click on create new product image button and verify new product image panel opened successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.ClickOnCreateNewProductImageAndVerifyNewProductImagePanel(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Create a new product image
                stepName = "Create a new product image";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.CreateProductImage(product, imageName, imageOrder, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Verify the product image displayed in the table
                stepName = "Verify the product image displayed in the Product Image Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.VerifyProductImageExists(imageName);
                testStep.SetOutput("ProductImage :" + imageName + " in the Product Image Grid");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Logout
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            }

            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                Assert.Fail();
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }

        /// <summary>
        /// Test BTA_98 : Edit a Product Image
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_98_LN_EditProductImage()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            string imageName = WebsiteData.ProductImageName + RandomDataHelper.RandomString(5);
            string imageOrder = WebsiteData.ProductImageOrder;
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Click on reward catalog and verify product images tab displayed 
                stepName = "Click on reward catalog and verify product images tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product images tab and verify product images panel displayed 
                stepName = "Click on product images tab and verify product images panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductImages);
                testStep.SetOutput("Product images panel Openedsuccessfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify the existence of category and create category if doesn't exist
                stepName = "Verify the existence of category and create category if doesn't exist";
                testStep = TestStepHelper.StartTestStep(testStep);
                rewardCatlog_productImagePage.VerifyCategoryExistanceAndCreateIfNotExists(product, out string outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify the existence of product and create product if doesn't exist
                stepName = "Verify the existence of product and create product if doesn't exist";
                testStep = TestStepHelper.StartTestStep(testStep);
                rewardCatlog_productImagePage.VerifyProductExistanceAndCreateIfNotExists(product, out outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Click on create new product image button and verify new product image panel opened successfully
                stepName = "Click on create new product image button and verify new product image panel opened successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.ClickOnCreateNewProductImageAndVerifyNewProductImagePanel(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Create a new product image
                stepName = "Create a new product image";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.CreateProductImage(product, imageName, imageOrder, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Verify the product image displayed in the table
                stepName = "Verify the product image displayed in the table";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.VerifyProductImageExists(imageName);
                testStep.SetOutput("ProductImage "+imageName+" displayed in the table");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Click on Edit icon of the newly created product image and verify edit product image panel opened
                stepName = "Click on Edit icon of the newly created product image and verify edit product image panel opened";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.ClickOnEditProductImageAndVerifyEditProductImagePanel(imageName, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

               #region Step12: Edit and verify the product image updated
                stepName = "Edit and verify the product image updated";
                string imageName_Updated = imageName + RandomDataHelper.RandomString(5);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.EditProductImage(imageName_Updated, out outMessage);
                testStep.SetOutput("Product Image:"+imageName+ "is Updated to : "+ outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Verify the product image displayed in the table
                stepName = "Verify the product image displayed in the table";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.VerifyProductImageExists(imageName);
                testStep.SetOutput("ProductImage: " + imageName + " displayed in the table");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Logout
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            }

            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                Assert.Fail();
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }
        /// <summary>
        /// Test BTA_99 : Delete a Product Image
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_99_LN_DeleteProductImage()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            string imageName = WebsiteData.ProductImageName + RandomDataHelper.RandomString(5);
            string imageOrder = WebsiteData.ProductImageOrder;
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Click on reward catalog and verify product images tab displayed 
                stepName = "Click on reward catalog and verify product images tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product images tab and verify product images panel displayed 
                stepName = "Click on product images tab and verify product images panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductImages);
                testStep.SetOutput("Product images panel Openedsuccessfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify the existence of category and create category if doesn't exist
                stepName = "Verify the existence of category and create category if doesn't exist";
                testStep = TestStepHelper.StartTestStep(testStep);
                rewardCatlog_productImagePage.VerifyCategoryExistanceAndCreateIfNotExists(product, out string outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify the existence of product and create product if doesn't exist
                stepName = "Verify the existence of product and create product if doesn't exist";
                testStep = TestStepHelper.StartTestStep(testStep);
                rewardCatlog_productImagePage.VerifyProductExistanceAndCreateIfNotExists(product, out outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Click on create new product image button and verify new product image panel opened successfully
                stepName = "Click on create new product image button and verify new product image panel opened successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.ClickOnCreateNewProductImageAndVerifyNewProductImagePanel(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Create a new product image
                stepName = "Create a new product image";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.CreateProductImage(product, imageName, imageOrder, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Verify the product image displayed in the table
                stepName = "Verify the product image displayed in the table";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.VerifyProductImageExists(imageName);
                testStep.SetOutput("ProductImage"+imageName+" displayed in the table");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Delete a product image
                stepName = "Delete a product image";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.DeleteProductImage(imageName, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Verify the product image displayed in the table
                stepName = "Verify the product image in the ProductImage Grid; Product Image Shouldn't appear";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = rewardCatlog_productImagePage.VerifyProductImageDoesNotExists(imageName, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Logout
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            }

            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                Assert.Fail();
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