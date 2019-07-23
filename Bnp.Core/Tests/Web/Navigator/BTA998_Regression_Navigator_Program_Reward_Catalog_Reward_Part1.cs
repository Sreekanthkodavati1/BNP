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
using System.Globalization;
namespace Bnp.Core.Tests.Web.Navigator
{

    /// <summary>
    /// Test BTA-997: Reward_Part1
    /// </summary> 
    [TestClass]
    public class BTA998_Regression_Navigator_Program_Reward_Catalog_Reward_Part1 : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// Test BTA_79: To delete reward
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_79_LN_Program_RewardCatalog_Reward_DeleteReward()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            string stepName = "";
            #endregion
            try
            {

                #region Object Initialization
                CategoryFields reward = new CategoryFields();
                var attName = data.AttributeAllContentType;
                product.SetType = "Product Name";
                reward.PredicateDropDown = CategoryFields.Predicates.Eq.ToString();
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName + RandomDataHelper.RandomString(4);
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

                #region Step5: Create new reward and Verify
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Name";
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward for product as " + reward.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_Product(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion             

                #region Step6: Delete Reward
                stepName = "Delete Reward " + reward.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_RewardsPage.SearchRewardBasedOnPredicate(reward.SetType, reward.PredicateDropDown, reward.Name);
                testStep.SetOutput(RewardCatalog_RewardsPage.DeleteReward(reward.SetType, reward.Name, reward.CategoryTypeValue));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Logout
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
        /// Test BTA_80: To create reward
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_80_LN_Program_RewardCatalog_Reward_CreateReward()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            string stepName = "";
            #endregion
            try
            {
                #region Object Initialization
                var attName = data.AttributeAllContentType;
                product.SetType = "Product Name";
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName + RandomDataHelper.RandomString(4);
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

                #region Step5: Create new reward and Verify
                CategoryFields reward = new CategoryFields
                {
                    Name = data.RewardName + RandomDataHelper.RandomString(5),
                    BalanceNeeded = "100",
                    CategoryTypeValue = product.Name,
                    SetType = "Reward Name"
                };
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward for product as " + reward.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_Product(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Logout
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
        /// Test BTA_81: To Edit reward
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_81_LN_Program_RewardCatalog_Reward_EditReward()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            var attName = data.AttributeAllContentType;
            CategoryFields reward = new CategoryFields();
            string stepName = "";
            #endregion

            try
            {
                #region Object Initialization
                //Product Values
                product.SetType = "Product Name";
                product.CategoryName = data.ProductCategoryName;
                //Reward Values
                reward.BalanceNeeded = "100";
                reward.SetType = "Reward Name";
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName + RandomDataHelper.RandomString(4);
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

                #region Step5: Create new reward and Verify
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Name";
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward as " + reward.Name + " Edit reward and save the same";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                RewardCatalog_RewardsPage.CreateReward_With_Product(reward);
                testStep.SetOutput(RewardCatalog_RewardsPage.EditReward(reward, product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Search and Edit Reward 11th reward
                stepName = "Search and Edit 11th Reward";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput("11th " + RewardCatalog_RewardsPage.EditAndVerify11thReward(reward, product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Logout
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
        /// Test BTA_82: To Clone reward
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_82_LN_Program_RewardCatalog_Reward_CloneReward()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            string stepName = "";
            #endregion
            try
            {
                #region Object Initialization
                var attName = data.AttributeAllContentType;
                product.SetType = "Product Name";
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName + RandomDataHelper.RandomString(4);
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

                #region Step5: Create new reward and Verify
                CategoryFields reward = new CategoryFields
                {
                    Name = data.RewardName + RandomDataHelper.RandomString(5),
                    BalanceNeeded = "100",
                    CategoryTypeValue = product.Name,
                    SetType = "Reward Name"
                };
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward for product as " + reward.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_Product(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Search and Clone Reward
                stepName = "Search and Clone Reward";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_RewardsPage.CloneAndVerifyReward(reward.SetType, reward.Name, reward.CategoryTypeValue));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Logout
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
        /// Verify all different validations while creating Regular Reward
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_77_LN_RegularRewardCreationValidations()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var RewardCatalog_ProductVariantsPage = new Navigator_Users_Program_RewardCatalog_ProductVariantsPage(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            CategoryFields reward = new CategoryFields();
            string stepName = "";
            #endregion

            try
            {
                #region Object Initialization
                var attName = data.AttributeAllContentType;
                product.SetType = "Product Name";
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName + RandomDataHelper.RandomString(4);
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

                #region Step4: Verify that error message: "Please provide a reward name." is displayed while creating Reward
                stepName = "Verify that error message: 'Please provide a reward name.' is displayed while creating Reward";
                reward.CategoryTypeValue = product.Name;
                reward.BalanceNeeded = reward.RewardBalanceNeeded;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                RewardCatalog_RewardsPage.CreateRewardWithoutNameAndVerifyErrorMessage(reward);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that error message: The error message: "Name cannot be more than 300 characters." while creating Reward
                stepName = "Verify that error message: The error message: 'Name cannot be more than 300 characters.' while creating Reward";
                reward.CategoryTypeValue = product.Name;
                reward.Name = RandomDataHelper.RandomString(301);
                reward.BalanceNeeded = reward.RewardBalanceNeeded;
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_RewardsPage.CreateRewardWithMoreThanThreeHundredCharactersAndVerifyErrorMessage(reward);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify that the error message: "Enter a non-negative value." is displayed when entered Alpha Charaters in BalanceNeeded while creating Reward
                stepName = "Verify that the error message: 'Enter a non - negative value.' is displayed when entered Alpha Charaters in BalanceNeeded while creating Reward";
                reward.CategoryTypeValue = product.Name;
                reward.Name = RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = RandomDataHelper.RandomString(4);
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_RewardsPage.CreateRewardByEnteringOtherThanNumericInBalanceNeededAndVerifyErrorMessage(reward);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify that the error message: "Enter a non-negative value." is displayed when entered Special Characters in BalanceNeeded while creating Reward
                stepName = "Verify that the error message: 'Enter a non - negative value.' is displayed when entered Special Characters in BalanceNeeded while creating Reward";
                reward.CategoryTypeValue = product.Name;
                reward.Name = RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "!@#%^&*";
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_RewardsPage.CreateRewardByEnteringOtherThanNumericInBalanceNeededAndVerifyErrorMessage(reward);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Verify that the error message: "Please provide a valid numeric value for balance needed." is displayed without BalanceNeeded while creating Reward
                stepName = "Verify that the error message: 'Please provide a valid numeric value for balance needed.' is displayed without BalanceNeeded while creating Reward";
                reward.CategoryTypeValue = product.Name;
                reward.Name = RandomDataHelper.RandomString(5);
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_RewardsPage.CreateRewardWithoutBalanceNeededAndVerifyErrorMessage(reward);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Verify that the error message: "CertificateTypeCode cannot be more than 50 characters." is displayed while creating Reward
                stepName = "Verify that the error message: 'CertificateTypeCode cannot be more than 50 characters.' is displayed while creating Reward";
                reward.CategoryTypeValue = product.Name;
                reward.Name = RandomDataHelper.RandomString(5);
                reward.CertificateTypeCode = RandomDataHelper.RandomString(51);
                reward.BalanceNeeded = reward.RewardBalanceNeeded;
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_RewardsPage.CreateRewardWithFiftyCharsCertificateTypeCodeAndVerifyErrorMessage(reward);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Verify text box labled "Redemption Time Limit (Minutes)"
                stepName = "Verify text box labled 'Redemption Time Limit(Minutes)'";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_RewardsPage.VerifyRedemptionTimeLimitLabel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Logout
                stepName = "Logout from Rewards page";
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
