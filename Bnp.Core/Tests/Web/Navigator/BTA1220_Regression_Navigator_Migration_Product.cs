using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.RewardCatalog;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA_1220_Regression_Navigator_Migration_Product : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// BTA 73 : This method is used to Migrate Product With AttributeSet and verify the Product With AttributeSet migrated successfully or not
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_73_LN_Migration_Product_With_AttributeSet()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            Migration Migration = new Migration(driverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
            var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
            var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
            var data = new NonAdminUserData(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields rate = new CategoryFields();
            rate.PredicateDropDown = CategoryFields.Predicates.Ne.ToString();
            string stepName = "";
            #endregion

            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Create attribute for Product content type
                stepName = "Create attribute for Product content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                testStep.SetOutput(attributesPage.CreateNewAttribute(Migration.MigrationSets.Migration_Product_AttributeSet.ToString()));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Product_AttributeSet.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Product_AttributeSet.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForProductAttribute(Migration.MigrationSets.Migration_Product_AttributeSet.ToString(), out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Verify Attribute Migrated on Attribute Page
                stepName = "Verify Attribute Migrated on Attribute Page";
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                attributesPage.VerifyAttributeExists(Migration.MigrationSets.Migration_Product_AttributeSet.ToString());
                testStep.SetOutput(" Attribute :" + Migration.MigrationSets.Migration_Product_AttributeSet.ToString() + " Migrated Successfully and appeared on Attribute Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Logout
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, false, DriverContext.SendScreenshotImageContent("WEB"));
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
        /// BTA 75 : This method is used to Migrate Product With Image and Verify that the Exception is thrown when migrating image to an environment that doesn't have the associated product
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_75_LN_VerifyFinishedWithErrorsMessageShownWhileMigratingProductImage()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            Migration Migration = new Migration(driverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            string imageName = Migration.MigrationSets.Migration_ProductImage.ToString() + RandomDataHelper.RandomString(5);
            string imageOrder = WebsiteData.ProductImageOrder;
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string Status = "FinishedWithErrors";
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

                #region Step3: Select the program application and verify the program configuration panel displayed
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

                #region Step11: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_ProductImage.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_ProductImage.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForProductImage(imageName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18: Verify that the Finished with Errors status is displayed while migrating the image to an environment that doesn't have the associated product
                stepName = "Verify that the Finished with Errors status is displayed while migrating the image to an environment that doesn't have the associated product";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.VerifyErrorStatusInMigrationPage(Migration.BuildMigrationSetName, Status, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step19: "Verify The product no longer exists in the destination database error message while migrating product image";
                stepName = "Verify The product with name " + imageName + " no longer exists in the destination database error message while migrating product image";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.VerifyProductNoLongerExistsExceptionInMigrationViewItemsPage(Migration.BuildMigrationSetName, Status, out output, imageName);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step20: Logout
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, false, DriverContext.SendScreenshotImageContent("WEB"));
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
        /// BTA 76 : This method is used to Migrate Product and verify the Product is migrated successfully or not
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_76_LN_Migrate_Product_And_VerifyProductMigrated()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            Migration Migration = new Migration(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateProduct_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            product.Name = Migration.MigrationSets.Migration_Product.ToString() + RandomDataHelper.RandomString(4);
            string stepName = "";
            #endregion
            try
            {
                #region Object Initialization
                product.SetType = "Product Name";
                var attName = data.AttributeAllContentType;
                #endregion

                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin 
                stepName = "Login As User Admin and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Verify the existence of category and create category if doesn't exist
                stepName = "Verify the existence of category and create category if doesn't exist";
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateProduct_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.AttributeName = attName + contentType.ToString();
                product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Product.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Product.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForProductCategoryAndProduct(product.Name, product.CategoryName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Verify Product Migrated on Product Page
                stepName = "Verify Product Migrated on Product Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                RewardCatalog_ProductsPage.VerifyCreatedProduct(product.SetType, product.Name, product.CategoryName);
                testStep.SetOutput(" Product :" + Migration.MigrationSets.Migration_Product.ToString() + " Migrated Successfully and appeared on Product Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Logout
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, false, DriverContext.SendScreenshotImageContent("WEB"));
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
