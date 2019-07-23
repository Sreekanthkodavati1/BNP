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
    public class BTA1226_Regression_Navigator_Program_Components_LocationGroups : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// BTA_57 To Create new Location Group
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA57_Regression_Navigator_Program_Components_Location_Group_CreateLocationGroup()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(LGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Verify Id, Name, Created Date and Actions Columns in Location Groups tab         
                stepName = "Verify Id, Name, Created Date and Actions Columns in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Col_status = LocationGroup.VerifyLocationGroupAllColumn(out string ColumnStatus);
                testStep.SetOutput(ColumnStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Col_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Out_status = LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify that the grid has a column for Actions in Location Group tab with All Options           
                stepName = "Verify that the grid has a column for Actions in Location Group tab with All Options";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool options = LocationGroup.VerifyLocationGroupActionColumnOptionsTab(LGroup.LocationGroupName, out string option);
                testStep.SetOutput(option);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, options, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify Location Group name and description Validation             
                stepName = "Verify Location Group name and description Validation";
                testStep = TestStepHelper.StartTestStep(testStep);
                string RandomStr = RandomDataHelper.RandomString(33);
                LGroup.LocationGroupName = NonAdminUserData.LocationGroupName + RandomStr;
                LGroup.Description = "" + RandomDataHelper.RandomString(1001);
                LocationGroup.CreateNewLocationGroup(LGroup);
                bool Desc_Length = LocationGroup.VerifyLocationGroupDescriptionLength(LGroup.LocationGroupName, out string Desc_Output);
                testStep.SetOutput(Desc_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Desc_Length, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Logout
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
        /// BTA_58 Validation and verifications of Location Group
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA58_Regression_Navigator_Program_Components_Location_Group_FieldValidation()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Verify New Location Group Button and Cancel Button in Location Groups Tab              
                stepName = "Verify New Location Group Button and Cancel Button in Location Groups Tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                bool result=LocationGroup.VerifyButtonExists("New Location Group", out string OutResult);
                bool CancelResult = false;
                if (result) { CancelResult = LocationGroup.VerifyCancelButton(LGroup, "Cancel", out string CancelStatus); testStep.SetOutput(CancelStatus); }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, CancelResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion             

                #region Step4: Verify that clicking the Save button performs field validation.               
                stepName = "Verify that clicking the Save button performs field validation";
                testStep = TestStepHelper.StartTestStep(testStep);
                LocationGroup.CreateNewLocationGroup(LGroup);
                bool SaveResult = LocationGroup.VerifyFieldValidationWithSaveButton(LGroup, "Save", out string SaveStatus);
                testStep.SetOutput(SaveStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, SaveResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that the Name field is required in New Location Group             
                stepName = "Verify that the Name field is required in New Location Group ";
                testStep = TestStepHelper.StartTestStep(testStep);
                LGroup.LocationGroupName = "";
                LGroup.Description = "Write Data In Description Area";
                bool NameResult = LocationGroup.VerifyNameField(LGroup,"Save", out string NameError);
                testStep.SetOutput(NameError);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, NameResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify that No Error Message Appear when switching to other tab from components                
                stepName = "Verify that No Error Message Appear when switching to other tab from components";
                testStep = TestStepHelper.StartTestStep(testStep);
                LocationGroup.CreateNewLocationGroup(LGroup);
                bool ErrorResult = LocationGroup.VerifyNoErrorMessageAppear(LGroup.LocationGroupName, out string NoError);
                testStep.SetOutput(NoError);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, ErrorResult, DriverContext.SendScreenshotImageContent("WEB"));
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
        /// BTA_59 Edit and Verify Location Group
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA59_Regression_Navigator_Program_Components_Location_Group_EditLocationGroups()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(LGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Out_status = LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that Edit Location Group window is with fields and action buttons             
                stepName = "Verify that Edit Location Group window is with fields and action buttons";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool EditResult = LocationGroup.VerifyEditWindow(LGroup, "Edit", out string EditWindow);
                testStep.SetOutput(EditWindow);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, EditResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify that the Name field is not editable in the Edit Location Group window             
                stepName = "Verify that the Name field is not editable in the Edit Location Group window";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool NameResult = LocationGroup.VerifyNameFieldInEditWindow(LGroup, "Edit", out string NameError);
                testStep.SetOutput(NameError);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, NameResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify that the Save button updates the location group definition             
                stepName = "Verify that the Save button updates the location group definition";
                testStep = TestStepHelper.StartTestStep(testStep);
                LGroup.Description = "Write Data In Description Area after edit and save";
                testStep.SetOutput(LocationGroup.EditLocationGroup(LGroup, "Edit"));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Verify that clicking the cancel button will not save any edits made to the Location Group definition             
                stepName = "Verify that clicking the cancel button will not save any edits made to the Location Group definition";
                testStep = TestStepHelper.StartTestStep(testStep);
                string randomStr1 = RandomDataHelper.RandomString(4);
                LGroup.LocationGroupName = NonAdminUserData.LocationGroupName + randomStr1;
                LGroup.Description = "Write Data In Description Area";
                LocationGroup.VerifyCancelButton(LGroup, "Cancel",out string cancelStatus);
                bool LGroupExists = !LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string ExistsStatus);
                testStep.SetOutput(cancelStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, LGroupExists, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Logout
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
        /// BTA_60 Delete and Verify Location Group
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA60_Regression_Navigator_Program_Components_Location_Group_DeleteLocationGroups()
        {
            #region Object Initialization
            CategoryFields product = new CategoryFields();
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var attName = data.AttributeAllContentType;
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + RandomDataHelper.RandomString(5),
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(LGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                bool Out_status = LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create New Category with Product and Verify
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

                #region Step6: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName + RandomDataHelper.RandomString(4);
                product.AttributeName = attName + contentType.ToString();
                product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
                product.SetType = "Product Name";
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Create new reward with location group selected and Verify
                CategoryFields reward = new CategoryFields
                {
                    Name = data.RewardName + RandomDataHelper.RandomString(5),
                    LocationGroupName = LGroup.LocationGroupName,
                    BalanceNeeded = "100",
                    CategoryTypeValue = product.Name,
                    SetType = "Reward Name"
                };
                var date = DateTime.Now;
                reward.StartDate = DateHelper.GetDate("Current");
                reward.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward with location group selected " + reward.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_LocationGroup(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                
                #region Step4: Verify location group can be deleted after reward was modified to not be linked to the location group           
                stepName = "Verify location group can be deleted after reward was modified to not be linked to the location group";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                LGroup.SetType = "Reward Name";
                bool delStatus = LocationGroup.DeleteLocationGroupAfterRewardModification(LGroup, "Ok", out string DeleteStatus);
                testStep.SetOutput(DeleteStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, delStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify location group can be deleted if the reward linked was deleted before  
                
                #region Step1: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                CategoryFields NewLGroup = new CategoryFields
                {
                    LocationGroupName = NonAdminUserData.LocationGroupName + RandomDataHelper.RandomString(5),
                    Description = "Write Data In Description Area"
                };
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(NewLGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                

                #region Step2: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                bool Out_str = LocationGroup.VerifyLocationGroupExists(NewLGroup.LocationGroupName, out string Out_Msg);
                testStep.SetOutput(Out_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_str, DriverContext.SendScreenshotImageContent("WEB"));
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
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName + RandomDataHelper.RandomString(4);
                product.AttributeName = attName + contentType.ToString();
                product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
                product.SetType = "Product Name";
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create new reward with location group selected and Verify
                CategoryFields rewardNew = new CategoryFields
                {
                    Name = data.RewardName + RandomDataHelper.RandomString(5),
                    LocationGroupName = NewLGroup.LocationGroupName,
                    BalanceNeeded = "100",
                    CategoryTypeValue = product.Name,
                    SetType = "Reward Name"
                };
                rewardNew.StartDate = DateHelper.GetDate("Current");
                rewardNew.ExpiryDate = DateHelper.GetDate("Future");
                stepName = "Create new reward with location group selected " + rewardNew.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                reward.AttributeName = attName + contentType.ToString();
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_LocationGroup(rewardNew));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify location group can be deleted if the reward linked was deleted before  
                stepName = "Verify location group can be deleted if the reward linked was deleted before";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                LGroup.SetType = "Reward Name";
                bool deleteStatus = LocationGroup.DeleteLocationGroupAfterDeleteReward(NewLGroup, "Ok", out string DelStatus);
                testStep.SetOutput(DelStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, deleteStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

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
        /// BTA_61 Clone and Verify Location Group
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA61_Regression_Navigator_Program_Components_Location_Group_CloneLocationGroups()
        {
            #region Object Initialization
            CategoryFields product = new CategoryFields();
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var attName = data.AttributeAllContentType;
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(LGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                bool Out_status = LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify Clone Window of Location Group In Location Groups Tab
                stepName = "Verify Clone Window of Location Group In Location Groups Tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Window_status=LocationGroup.VerifyCloneWindow(LGroup, "Clone", out string Msg);
                testStep.SetOutput(Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Window_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Clone and Verify Location Group In Location Groups Tab          
                stepName = "Clone and Verify Location Group In Location Groups Tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                randomStr = RandomDataHelper.RandomString(4);
                CategoryFields CloneLGroup = new CategoryFields
                {
                    LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                    Description = "For cloning orignal Location Group Write Data In Description Area"
                };
                LocationGroup.CloneLocationGroup(CloneLGroup, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
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
        /// BTA_63 To Create new Location Group and Verify ShowStores Button
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA63_Regression_Navigator_Location_Group_Click_On_ShowStores_And_Verify_Message()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(LGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Out_status = LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on Criteria Builder button and Verify "Show Stores" Button
                stepName = "Click on Criteria Builder button and Verify Show Stores Button";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VerifyShowStoresButton(LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click on Show Stores button and Verify Message
                stepName = "Click on Show Stores button and Verify Message";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.ClickOnShowStoresAndVerifyMessage(LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Logout
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
        /// BTA_64 To Create And Verify Criteria
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA64_Regression_Navigator_Location_Group_Create_And_Verify_Criteria()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            var StorePage = new Navigator_Users_Program_Components_StoresPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            testCase = new TestCase(TestContext.TestName);
            Store store = new Store
            {
                StoreNumber = RandomDataHelper.RandomNumber(4),
                StoreName ="Auto_Store" + RandomDataHelper.RandomString(4),
                PhoneNumber = RandomDataHelper.RandomNumber(10),
                City = "TestCity",
                State = "TestState",
                Country = "TestCountry",
                Address = "TestAddress"
            };
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Create new Store             
                stepName = " Create new Store";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Stores);
                StorePage.CreateStore(store, out string msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(LGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Out_status = LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Create The Criteria For Newly created LocationGroup
                stepName = "Create The Criteria For Newly created LocationGroup";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.CreateCriteria(store.StoreNumber,LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify Newly Created Criteria
                stepName = " Verify Newly Created Criteria";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VerifyCriteria(LGroup.LocationGroupName,store.StoreNumber, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify LocationGroup In The Database 
                stepName = "Verify LocationGroup In The Database";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VeriyLocationGroupInDB(LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Logout
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
        /// BTA_62 To Validation Of Inputfields Buttons And Selectfields In LocationGroup Page
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA62_Regression_Navigator_Validation_Of_Inputfields_Buttons_And_Selectfields_In_LocationGroup_Page()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            var StorePage = new Navigator_Users_Program_Components_StoresPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            NonAdminUserData nonAdminUserData = new NonAdminUserData(DriverContext);
            testCase = new TestCase(TestContext.TestName);
            Store store = new Store
            {
                StoreNumber = RandomDataHelper.RandomString(4) + RandomDataHelper.RandomNumber(2) + RandomDataHelper.RandomSpecialCharactersString(2),
                StoreName = nonAdminUserData.StoreName + RandomDataHelper.RandomString(4),
                PhoneNumber = RandomDataHelper.RandomNumber(10),
                City = "TestCity",
                State = "TestState",
                Country = "TestCountry",
                Address = "TestAddress"
            };
            CategoryFields LGroup = new CategoryFields
            {
                LocationGroupName = NonAdminUserData.LocationGroupName + randomStr,
                Description = "Write Data In Description Area"
            };
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

                #region Step3: Create new Store             
                stepName = " Create new Store";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Stores);
                StorePage.CreateStore(store, out string msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Location Group              
                stepName = " Create new Location Group";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LocationGroups);
                testStep.SetOutput(LocationGroup.CreateNewLocationGroup(LGroup));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that the newly added Location Group gets added in the grid in Location Groups tab           
                stepName = "Verify that the newly added Location Group gets added in the grid in Location Groups tab";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Out_status = LocationGroup.VerifyLocationGroupExists(LGroup.LocationGroupName, out string Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Validate The Criteria input field allow Special characters,Numbers and Text
                stepName = "Validate The Criteria input field allow special characters,Numbers and Text";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.CreateCriteria(store.StoreNumber, LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput("Successfully Created Criteria with Special characters,Numbers and Text as " + store.StoreNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify Newly Created Criteria
                stepName = " Verify Newly Created Criteria";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VerifyCriteria(LGroup.LocationGroupName, store.StoreNumber, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Verify The Criteria Builder Button
                stepName = " Verify The Criteria Builder Button";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VerifyCriteriaBuilderButton(LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Click on Criteria Builder button and Verify "Save" Button
                stepName = "Click on Criteria Builder button and Verify Save Button";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VerifyCriteriaBuilderSaveButton(LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Click on Criteria Builder button and Verify "Cancel" Button
                stepName = "Click on Criteria Builder button and Verify Cancel Button";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VerifyCriteriaBuilderCancelButton(LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Verify The Criteria Builder ConditionProperty Dropdown Values 
                stepName = "Verify The Criteria Builder ConditionProperty Dropdown Values";
                testStep = TestStepHelper.StartTestStep(testStep);
                List<string> CriteriaConditionList = new List<string>(new string[] { "Address Line One", "Address Line Two", "Attributes", "Brand Name", "Brand Store Number", "City", "Close Date", "ContentAttributes"
                ,"Contents","ContentType","Country","County","Create Date","District","Id","Latitude","Longitude","Long User Field","Open Date","Phone Number","Region",
                "State Or Province","Status","Store Id","Store Name","Store Number","Store Type","String User Field","Update Date","Zip Or Postal Code","Zone"});
                Out_status = LocationGroup.VerifyCriteriaBuilderConditionPropertyDropdownValues(CriteriaConditionList, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify The Criteria Builder conditionOperator Dropdown Values 
                stepName = "Verify The Criteria Builder conditionOperator Dropdown Values";
                testStep = TestStepHelper.StartTestStep(testStep);
                List<string> CriteriaConditionOperator = new List<string>(new string[] { "Less than", "Less than or equal to", "Equal to", "Greater than or equal to", "Greater than", "Not equal to" });
                Out_status = LocationGroup.VerifyCriteriaBuilderConditionOperatorDropdownValues(CriteriaConditionOperator, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Click on Criteria Builder button and Verify "Formula"
                stepName = "Click on Criteria Builder button and Verify Formula Button";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_status = LocationGroup.VerifyCriteriaBuilderFormula(LGroup.LocationGroupName, out Out_msg);
                testStep.SetOutput(Out_msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
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