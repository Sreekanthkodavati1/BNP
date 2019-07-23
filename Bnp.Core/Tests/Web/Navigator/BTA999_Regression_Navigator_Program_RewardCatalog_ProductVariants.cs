using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
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
    /// Test to test BTA-999  : Create, Edit and Delete Product Variants
    /// </summary>
    [TestClass]
    public class BTA999_Regression_Navigator_Program_RewardCatalog_ProductVariants : ProjectTestBase
    {
        Login login = new Login();
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        readonly string password = NavigatorUsers.NavigatorPassword;

        /// <summary>
        /// Test BTA_94A : Create a Product Variant with existing category and product
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
         public void BTA_94A_LN_CreateProductVariant()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            CategoryFields productVariants = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            productVariants.Name = WebsiteData.ProductVariantName + RandomDataHelper.RandomString(5);
            productVariants.CategoryTypeValue = product.Name;
            productVariants.CategoryName = product.CategoryName;
            productVariants.Quantity = WebsiteData.Quantity+RandomDataHelper.RandomNumber(2);
            productVariants.PartNumber = WebsiteData.PartNumber + RandomDataHelper.RandomString(2);
            productVariants.QuantityThreshold = WebsiteData.QuantityThreshold + productVariants.Quantity;
            productVariants.VariantOrder = WebsiteData.VariantOrder;
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

                #region Step4: Click on reward catalog and verify products tab displayed 
                stepName = "Click on reward catalog and verify products tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product variants tab and verify product variants panel displayed 
                stepName = "Click on product variants tab and verify product variants panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductVariants);
                testStep.SetOutput("Product variants panel Openedsuccessfully");
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

                #region Step8: Click on create new product variant button and verify new product variant panel opened successfully
                stepName = "Click on create new product variant button and verify new product variant panel opened successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.ClickOnCreateNewProductVariantAndVerifyNewProductVariantPanel(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Create new productvariants and Verify 
                stepName = "Create new productvariants for Category as " + productVariants.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_ProductVariantsPage.CreateProductVariants_With_ProductCategory_And_ProductName(productVariants));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Logout
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
        /// Test BTA_94B : Create a Product Variant form DB
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_94B_LN_CreateProductVariantFromDB()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            CategoryFields productVariants = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            productVariants.Name = "DB"+WebsiteData.ProductVariantName + RandomDataHelper.RandomString(5);
            productVariants.CategoryTypeValue = product.Name;
            productVariants.CategoryName = product.CategoryName;
            productVariants.Quantity = WebsiteData.Quantity+ RandomDataHelper.RandomNumber(2);
            productVariants.PartNumber = WebsiteData.PartNumber + RandomDataHelper.RandomString(2);
            productVariants.QuantityThreshold = WebsiteData.QuantityThreshold+ productVariants.Quantity;
            productVariants.VariantOrder = WebsiteData.VariantOrder;
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

                #region Step4: Click on reward catalog and verify products tab displayed 
                stepName = "Click on reward catalog and verify products tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product variants tab and verify product variants panel displayed 
                stepName = "Click on product variants tab and verify product variants panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductVariants);
                testStep.SetOutput("Product variants panel Openedsuccessfully");
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

                #region Step8: Create a product variant from db
                stepName = "Create a product variant from db";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.CreateProductVariantFromDB(productVariants,out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Verify the product variant is present in the web page
                stepName = "Verify the product variant is present in the web page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.VerifyProductVariantDisplayed(productVariants, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Logout
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
        /// Test BTA_94C : Create a Product Variant with a new product
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
         public void BTA_94C_LN_CreateProductVariantWithANewProduct()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            CategoryFields productVariants = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
            product.CategoryName = WebsiteData.ProductCategoryName;
            var attName = WebsiteData.AttributeAllContentType;
            product.SetType = "Product Name";
            product.Name = WebsiteData.ProductName + RandomDataHelper.RandomString(4);
            product.AttributeName = attName + contentType.ToString();
            product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
            productVariants.Name = WebsiteData.ProductVariantName + RandomDataHelper.RandomString(5);
            productVariants.CategoryTypeValue = product.Name;
            productVariants.CategoryName = product.CategoryName;
            productVariants.Quantity = WebsiteData.Quantity;
            productVariants.PartNumber = WebsiteData.PartNumber + RandomDataHelper.RandomString(2);
            productVariants.QuantityThreshold = WebsiteData.QuantityThreshold;
            productVariants.VariantOrder = WebsiteData.VariantOrder;
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

                #region Step4: Click on reward catalog and verify products tab displayed 
                stepName = "Click on reward catalog and verify products tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product variants tab and verify product variants panel displayed 
                stepName = "Click on product variants tab and verify product variants panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductVariants);
                testStep.SetOutput("Product variants panel Openedsuccessfully");
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

                #region Step7: Validate the presence of the product in the dropdown
                stepName = "Validate the presence of the product in the dropdown";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus= rewardCatlog_productImagePage.ValidatesProductExistance(product, out outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Click on create new product variant button and verify new product variant panel opened successfully
                stepName = "Click on create new product variant button and verify new product variant panel opened successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.ClickOnCreateNewProductVariantAndVerifyNewProductVariantPanel(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Create new productvariants and Verify 
                stepName = "Create new productvariants for Category as " + productVariants.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_ProductVariantsPage.CreateProductVariants_With_ProductCategory_And_ProductName(productVariants));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Logout
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
        /// Test BTA_95 : Delete a Product Variant 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_95_LN_DeleteProductVariant()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            CategoryFields productVariants = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            productVariants.Name = WebsiteData.ProductVariantName + RandomDataHelper.RandomString(5);
            productVariants.CategoryTypeValue = product.Name;
            productVariants.CategoryName = product.CategoryName;
            productVariants.Quantity = WebsiteData.Quantity;
            productVariants.PartNumber = WebsiteData.PartNumber + RandomDataHelper.RandomString(2);
            productVariants.QuantityThreshold = WebsiteData.QuantityThreshold;
            productVariants.VariantOrder = WebsiteData.VariantOrder;
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

                #region Step4: Click on reward catalog and verify products tab displayed 
                stepName = "Click on reward catalog and verify products tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product variants tab and verify product variants panel displayed 
                stepName = "Click on product variants tab and verify product variants panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductVariants);
                testStep.SetOutput("Product variants panel Openedsuccessfully");
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

                #region Step8: Click on create new product variant button and verify new product variant panel opened successfully
                stepName = "Click on create new product variant button and verify new product variant panel opened successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.ClickOnCreateNewProductVariantAndVerifyNewProductVariantPanel(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Create new productvariants and Verify 
                stepName = "Create new product variant for Category as " + productVariants.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_ProductVariantsPage.CreateProductVariants_With_ProductCategory_And_ProductName(productVariants));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Delete a product variant
                stepName = "Delete a product variant";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.DeleteProductVariant(productVariants, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Logout from User page
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
        /// Test BTA_96 : Edit a Product Variant 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_96_LN_EditProductVariant()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            CategoryFields productVariants = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            product.CategoryName = WebsiteData.ProductCategoryName;
            product.Name = WebsiteData.ProductName;
            productVariants.Name = WebsiteData.ProductVariantName + RandomDataHelper.RandomString(5);
            productVariants.CategoryTypeValue = product.Name;
            productVariants.CategoryName = product.CategoryName;
            productVariants.Quantity = WebsiteData.Quantity+ RandomDataHelper.RandomNumber(2);
            productVariants.PartNumber = WebsiteData.PartNumber + RandomDataHelper.RandomString(2);
            productVariants.QuantityThreshold = WebsiteData.QuantityThreshold+ productVariants.Quantity;
            productVariants.VariantOrder = WebsiteData.VariantOrder;
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

                #region Step4: Click on reward catalog and verify products tab displayed 
                stepName = "Click on reward catalog and verify products tab displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                testStep.SetOutput("Reward catalog panel opened successfully with all the tabs including product image");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on product variants tab and verify product variants panel displayed 
                stepName = "Click on product variants tab and verify product variants panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ProductVariants);
                testStep.SetOutput("Product variants panel Openedsuccessfully");
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

                #region Step8: Click on create new product variant button and verify new product variant panel opened successfully
                stepName = "Click on create new product variant button and verify new product variant panel opened successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.ClickOnCreateNewProductVariantAndVerifyNewProductVariantPanel(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Create new productvariants and Verify 
                stepName = "Create new productvariants for Category as " + productVariants.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_ProductVariantsPage.CreateProductVariants_With_ProductCategory_And_ProductName(productVariants));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Click on Edit icon of the newly created product image and verify edit product image panel opened
                stepName = "Click on Edit icon of the newly created product image and verify edit product image panel opened";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.ClickOnEditProductVariantAndVerifyEditProductVariantPanel(productVariants.Name, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Edit and verify the product image updated
                stepName = "Edit and verify the product image updated";
                productVariants.Name = productVariants.Name + "_updated";
                productVariants.PartNumber = productVariants.PartNumber + "_updated";
                productVariants.Quantity = (Convert.ToInt32(productVariants.Quantity) - 1).ToString();
                productVariants.QuantityThreshold = (Convert.ToInt32(productVariants.QuantityThreshold) - 1).ToString();
                productVariants.VariantOrder = (Convert.ToInt32(productVariants.VariantOrder) + 1).ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.EditProductVariant(productVariants, out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Verify the product image displayed in the table
                stepName = "Verify the product image displayed in the table";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = RewardCatalog_ProductVariantsPage.VerifyproductVariantsExists(productVariants);
                testStep.SetOutput("Poduct variant " + productVariants.Name + " displayed in the table");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Logout from User page
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
