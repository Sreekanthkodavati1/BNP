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
    /// Test BTA-997: Reward_Part2
    /// </summary> 
    [TestClass]
    public class BTA1036_Regression_Navigator__Program_Reward_Catalog_Reward_Part2 : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// Test BTA_83: To Create Payment type Reward validations
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_83_LN_Program_RewardCatalog_Reward_CreatePaymentTypeRewardValidations()
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

                #region Step5: Create new reward with reward type as payment
                CategoryFields reward = new CategoryFields
                {
                    Name = data.RewardName + RandomDataHelper.RandomString(5),
                    BalanceNeeded = "100",
                    CategoryTypeValue = product.Name,
                    SetType = "Reward Type"
                };
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward with reward type as payment for product as " + product.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateAndVerifyRewardWithRewardTypeAsPayment(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify Error Message For Payment Reward Without Tier Only Payment Reward
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Type";
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Verify Error Message For Payment Rewards Without Tier Only Payment Reward";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                testStep.SetOutput(RewardCatalog_RewardsPage.VerifyErrorMessageForPaymentRewards(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                

                #region Step7: Verify Error Message Can not set a Payment reward to a tier when one exists without a tier
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Type";
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Verify Error Message Can not set a Payment reward to a tier when one exists without a tier";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                reward.TierTypeValue = CategoryFields.TierType.Standard.ToString();
                testStep.SetOutput(RewardCatalog_RewardsPage.VerifyErrorMessageForPaymentRewards(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Create Payment type Reward with tier selected 
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Type";
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create Payment type Reward with tier selected";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                reward.TierTypeValue = CategoryFields.TierType.Standard.ToString();
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateAndVerifyRewardWithRewardTypeAsPaymentWithTierSelected(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Verify Error Message Payment reward tiers must not overlap
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Type";
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Verify Error Message Can not set a Payment reward to a tier when one exists without a tier";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                reward.TierTypeValue = CategoryFields.TierType.Standard.ToString();
                testStep.SetOutput(RewardCatalog_RewardsPage.VerifyErrorMessageForPaymentRewards(reward));
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
        /// Test BTA_84: To create Reward with Reward Type as Payment with Tier and save reward as default reward
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_84_LN_Program_RewardCatalog_Reward_CreatePaymentTypeRewardWithDifferentTierAndValidation()
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

                #region Step5:Create new reward with reward type as payment with Standard tier Selected
                CategoryFields reward = new CategoryFields();
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Tier";
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward with reward type as payment with standard tier Selected " + reward.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                reward.TierTypeValue = CategoryFields.TierType.Standard.ToString();
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateAndVerifyRewardWithRewardTypeAsPaymentAndForDifferentTierSelected(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Create new reward with reward type as payment with Silver tier Selected
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Tier";
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                reward.TierTypeValue = CategoryFields.TierType.Silver.ToString();
                stepName = "Create new reward with reward type as payment with " + reward.TierTypeValue + " tier Selected " + reward.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateAndVerifyRewardWithRewardTypeAsPaymentAndForDifferentTierSelected(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                

                //#region Step5:Verify and save reward in Default Rewards Page
                //stepName = "Verify and save reward in Default Rewards Page " + reward.Name;
                //testStep = TestStepHelper.StartTestStep(testStep);
                //navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                //Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.DefaultRewards);
                //testStep.SetOutput(RewardCatalog_RewardsPage.VerifyAndSaveTierRewardInDefaultRewardPage(reward.Name));
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion 

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
        /// Test BTA_85: To Create reward and chanege  "Catalog End Date" to something in the past fo rone of the reward. 
        /// Validate Expired Reward
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_85_LN_Program_RewardCatalog_Reward_ValidateExpiredReward()
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

                #region Step5: To Create reward and chanege  "Catalog End Date" to something in the past fo rone of the reward
                CategoryFields reward = new CategoryFields();
                var date = DateTime.Now;
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Tier";
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Current");
                stepName = "To Create reward and chanege  Catalog End Date to something in the past fo rone of the reward. " + reward.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                reward.TierTypeValue = CategoryFields.TierType.Silver.ToString();
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateAndVerifyRewardWithRewardTypeAsPaymentAndForDifferentTierSelected(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion  

                #region Step5:Verify Expired reward in Default Rewards Page
                stepName = "Verify Expired reward in Default Rewards Page" + reward.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.DefaultRewards);
                RewardCatalog_RewardsPage.VerifyRewardInDefaultReward(reward.Name, out string message);
                testStep.SetOutput(message);
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
        /// Test BTA_86: To Create Inactive Rewards with Reward Type as Payment with tier selected and verify it
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_86_LN_Program_RewardCatalog_Reward_ValidateActiveInActiveRewards()
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

                #region Step6:Create new reward InActive Reward with reward type as payment with Silver tier Selected
                CategoryFields reward = new CategoryFields();
                var date = DateTime.Now;
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Tier";
                reward.TierTypeValue = CategoryFields.TierType.Silver.ToString();
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create InActive reward with reward type as payment with " + reward.TierTypeValue + " tier Selected " + reward.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.ConversionRate = ".9";
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateInActiveRewardAndVerify(reward));
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
        /// Test BTA_87: Create reward with reward name as max character length and verify the name is partially hidden in default reward page
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_87_LN_VerifyRewardNameIsPartiallyHiddenWhenRewardNameIsTooLong()
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

                #region Step5:Create new reward with reward type as payment with Silver tier Selected
                CategoryFields reward = new CategoryFields();
                reward.Name = "Auto_Reward_" + RandomDataHelper.RandomString(288);
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Name";
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward with reward type as payment with standard tier Selected " + reward.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                reward.RewardTypeValue = CategoryFields.RewardType.Regular.ToString();
                reward.ConversionRate = ".9";
                reward.TierTypeValue = CategoryFields.TierType.Silver.ToString();
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateRegularRewardWithTier(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Navigate to Default Reward tab and verify the created reward reward is present
                stepName = "Navigate to Default Reward tab and verify the created reward reward is present";
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.DefaultRewards);
                bool stepStatus = RewardCatalog_RewardsPage.VerifyRewardIsPresentInDefaultRewardPageAndCanBeSelectable("Silver", reward.Name, out string outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
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
        /// Test BTA_78: To validate all different Error Messages while creating Reward with Tier
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_78_LN_RewardWithTierCreationValidations()
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
            var rewardCatlog_productImagePage = new Navigator_Users_Program_RewardCatalog_ProductImagesPage(DriverContext);
            CategoryFields reward = new CategoryFields();
            var navigator_Users_Program_Components_TiersPage = new Navigator_Users_Program_Components_TiersPage(DriverContext);
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

                #region Step5: Verify that the error message "Conversion Rate is required for Payment reward types" is displayed while creating Reward
                stepName = "Verify the error message 'Conversion Rate is required for Payment reward types' is displayed while creating Reward";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.CategoryTypeValue = product.Name;
                reward.Name = RandomDataHelper.RandomString(5);
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                reward.TierTypeValue = CategoryFields.TierType.Standard.ToString();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreatePaymentRewardWithoutConversionRateAndVerifyErrorMessage(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify 'Regular' and 'Payment' should be the only two available Reward Type options
                stepName = "Verify 'Regular' and 'Payment' should be the only two available Reward Type options";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                reward.RewardTypeValue = CategoryFields.RewardType.Payment.ToString();
                RewardCatalog_RewardsPage.VerifyRewardDropDownWhileCreatingReward(reward.RewardTypeValue);
                testStep.SetOutput(reward.RewardTypeValue = CategoryFields.RewardType.Regular.ToString());
                var stepOutput = RewardCatalog_RewardsPage.VerifyRewardDropDownWhileCreatingReward(reward.RewardTypeValue);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepOutput, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify A warning message "Tier "Tier Name" does not have any associated rewards. You must create rewards for this tier in order to make a default reward selection."is displayed in Default Rewards
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Tiers);
                string TierName = CategoryFields.TierType.Tier_Defaults.ToString();
                stepName = "Verify A warning message 'Tier \"" + TierName + "\" does not have any associated rewards. You must create rewards for this tier in order to make a default reward selection.' is displayed in Default Rewards";
                string ErrorText = "Tier \"" +  TierName  + "\" does not have any associated rewards. You must create rewards for this tier in order to make a default reward selection.";
                navigator_Users_Program_Components_TiersPage.CreateTierAndVerify(TierName, out string message);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.DefaultRewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.VerifyErrorMessagesInDefaultReward(ErrorText));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Verify A warning message "You have selected a default reward for one or more tiers, but one or more are missing a selection. When choosing default rewards, a reward should be selected for every available tier."is displayed in Default Rewards
                stepName = "Verify A warning message'You have selected a default reward for one or more tiers, but one or more are missing a selection.When choosing default rewards, a reward should be selected for every available tier.'is displayed in Default Rewards";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Tiers);
                string ErrorText1 = "You have selected a default reward for one or more tiers, but one or more are missing a selection. When choosing default rewards, a reward should be selected for every available tier.";
                navigator_Users_Program_Components_TiersPage.CreateTierAndVerify(TierName, out string outMessage);
                // Create new reward with Tier and Verify
                reward.Name = data.RewardName + RandomDataHelper.RandomString(5);
                reward.BalanceNeeded = reward.RewardBalanceNeeded;
                reward.CategoryTypeValue = product.Name;
                reward.TierTypeValue = CategoryFields.TierType.Tier_Defaults.ToString();
                reward.SetType = "Reward Name";
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                RewardCatalog_RewardsPage.CreateRewardAssoaciatedWithNewlyCreatedTier(reward,out string status);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.DefaultRewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.VerifyAndSelectRewardInDefaultReward(reward.Name));
                testStep.SetOutput(RewardCatalog_RewardsPage.VerifyErrorMessagesInDefaultReward(ErrorText1));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Logout
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
    }
}
