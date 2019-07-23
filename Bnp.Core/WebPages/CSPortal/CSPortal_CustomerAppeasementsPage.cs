using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bnp.Core.WebPages.CSPortal
{
    public class CSPortal_CustomerAppeasementsPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_CustomerAppeasementsPage(DriverContext driverContext)
       : base(driverContext)
        { }

        #region Customer Appeasements Page Element Loactors
        private readonly ElementLocator Textbox_Points = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtPoints')]");
        private readonly ElementLocator Button_Submit = new ElementLocator(Locator.XPath, "//div[@id='AwardPointsContainer']//a[contains(@id,'_btnSave')]");
        private readonly ElementLocator Select_Reward = new ElementLocator(Locator.XPath, "//span[contains(text(),'Reward')]//following::Select[1]");
        private readonly ElementLocator Select_Coupon = new ElementLocator(Locator.XPath, "//span[contains(text(),'Reward')]//following::Select[2]");
        private readonly ElementLocator Message_SuccessOnRewardAppeasment = new ElementLocator(Locator.XPath, "//*[contains(text(),'The Reward Appeasement has been successfully awarded.')]");
        private readonly ElementLocator Message_SuccessOnCouponAppeasment = new ElementLocator(Locator.XPath, "//*[contains(text(),'Coupon has been successfully awarded')]");
        #endregion

        /// <summary>
        /// Adding points to members
        /// </summary>
        /// <param name="points">points to add the member</param>
        /// <returns>Message with status</returns>
        public string AddingPointsToMember(string points, int maxPoints)
        {
            try
            {
                int value = Convert.ToInt16(points);
                var message = "Please enter a valid numeric value between 1 and " + maxPoints;
                Driver.GetElement(Textbox_Points).SendText(points);
                Driver.GetElement(Button_Submit).ClickElement();
                if (Driver.IsElementPresent(By.XPath("//span[contains(text(),'Please enter a valid numeric value between 1 and " + maxPoints + "')]")) && (value > maxPoints))
                {
                    return message;
                }
                else
                {
                    if (Driver.IsElementPresent(By.XPath("//div[contains(text(),'Points Added Successfully')]")))
                    {
                        message = "Points added successfully ;Added Points:" + points;
                        return message;
                    }
                    else
                    {
                        throw new Exception("Failed to add the points");
                    }
                }
            }
            catch
            {
                throw new Exception("Failed to add the points");
            }
        }

        /// <summary>
        /// Verify Points In AccountActivity Page
        /// </summary>
        /// <param name="points">points</param>
        /// <returns>returns true if points added in page otherwise return false</returns>
        public bool VerifyPointsInAccountActivityPage(string points)
        {
            if (Driver.IsElementPresent(By.XPath("//span[contains(text(),'" + points + "')]")))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Adding rewards to members
        /// </summary>
        /// <param name="rewardName">reward to add the member</param>
        /// <returns>Message with status</returns>
        public void AddingAppeasement(string SelectAppeasement, ElementLocator AppeasementType, string AppeasementName, string Submit)
        {
            try
            {
                ElementLocator Button_Submit = CustomerAppeasementsPage_Button_Custom_ElementLocatorXpath(Submit,"Submit");
                string selectAppeasement = "//span[contains(@id,'" + SelectAppeasement + "')]//select//option[1]";
                if (Driver.IsElementPresent(By.XPath(selectAppeasement)))
                {
                    SelectElement_AndSelectByText(AppeasementType, AppeasementName);
                    Click_OnButton(Button_Submit);
                    Driver.ScrollIntoMiddle(AppeasementType);
                }
            }
            catch
            {
                throw new Exception("Failed to add the points");
            }
        }

        public void AddingAppeasementInValidInput_NoValue(string ErrorMessage, string Submit,out string Message)
        {
            try
            {
                ElementLocator Button_Submit = CustomerAppeasementsPage_Button_Custom_ElementLocatorXpath(Submit, "Submit");
                Click_OnButton(Button_Submit); Message = "";
                if (Driver.IsElementPresent(ErrorFieldLevel(ErrorMessage),1))
                {
                    Driver.ScrollIntoMiddle(ErrorFieldLevel(ErrorMessage));
                    Message = "Error Message Appeared As Expected :"+ErrorMessage;
                }
                else
                {
                    throw new Exception("Failed to Verify Error Message:"+ErrorMessage);

                }
            }
            catch(Exception e)
            {
                throw new Exception("Failed to Verify Submit or Cancel Due Due to:"+e.Message);
            }
        }

        public void AddingAppeasementInValidInput_Cancel(string Type, out string Message)
        {
            try
            {
                 Message = "";
                 ElementLocator Button_Cancel = CustomerAppeasementsPage_Button_Custom_ElementLocatorXpath(Type, "Cancel");
                Click_OnButton(Button_Cancel);
                if (!Driver.FindElement(By.XPath("//span[text()='Please select a coupon.']")).Displayed)
                {
                    Message = "Error Message Removed As Expected";
                }
             
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Verify Cancel Due Due to:" + e.Message);
            }
        }


        /// <summary>
        /// Adding Rewards to member
        /// </summary>
        /// <param name="rewardName">Reward to be added to the member</param>
        /// <returns>Message with status</returns>
        public string AddRewardAppeasement(string rewardName, out string Messege)
        {
            try
            {
                AddingAppeasement("RewardSelect", Select_Reward, rewardName, "Reward Appeasement");
                if (Driver.IsElementPresent(Message_SuccessOnRewardAppeasment, 1))
                {
                    return Messege = "The Reward Appeasement has been successfully awarded.;Reward Details:" + rewardName;
                }
                else
                {
                    throw new Exception("Failed to Add Reward Appeasement in CSPortal");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Reward Appeasement in CSPortal");
            }
        }

        /// <summary>
        /// Adding Rewards to member
        /// </summary>
        /// <param name="rewardName">Reward to be added to the member</param>
        /// <returns>Message with status</returns>
        public bool AddRewardAppeasement_ForInActiveRule(string rewardName, out string Message)
        {          
           string Expected_ErrorMessage = "Execution of rule 'DefaultRewardAppeasementRule' was skipped because of reason 'RuleNotInDateRange'";
            try
            {
                AddingAppeasement("RewardSelect", Select_Reward, rewardName, "Reward Appeasement");
                ElementLocator _WarningMessage = new ElementLocator(Locator.XPath, "//div[@class='Negative']//span[contains(text(),'Execution of rule')]");

                if (Driver.IsElementPresent(_WarningMessage,1))
                {
                   string _AvailableWarningMessage= Driver.GetElement(_WarningMessage).GetTextContent();
                    if(_AvailableWarningMessage.Contains(Expected_ErrorMessage))
                    {
                        Message = "Expected Error Message appeared on Screen:" + _AvailableWarningMessage;return true;
                    }

                    throw new Exception("Failed to get Expected Error Message:" + Expected_ErrorMessage);
                    
                }
                throw new Exception("Failed to get Expected Error Message:" + Expected_ErrorMessage);

            }
            catch (Exception e)
            {
                throw new Exception("Failed to get Expected Error Message:" + Expected_ErrorMessage);
            }
        }

        /// <summary>
        /// Adding Coupon to member
        /// </summary>
        /// <param name="couponName">couponName to be added to the member</param>
        /// <returns>Message with status</returns>
        public string AddCouponAppeasement(string couponName, out string Messege)
        {
            try
            {
                AddingAppeasement("CouponSelect", Select_Coupon, couponName, "Coupon Appeasement");
                if (Driver.IsElementPresent(Message_SuccessOnCouponAppeasment, 1))
                {
                    return Messege = "The Coupon Appeasement has been successfully awarded.;Coupon Details:" + couponName;
                }
                else
                {
                    throw new Exception("Failed to Add Coupon Appeasement in CSPortal");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Coupon Appeasement in CSPortal");
            }
        }

        public static ElementLocator CustomerAppeasementsPage_Button_Custom_ElementLocatorXpath(string LableName,string ButtonsName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, "//h2[contains(text(),'" + LableName + "')]//following::a[contains(text(),'"+ButtonsName+"')]");
            return Button_Custom_ElementLocatorXpath;
        }
        public TestStep AddingAppeasementInValidInput_ForCoupon(string CouponName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Clicking on Submit With Out Selecting Coupon: "+CouponName;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddingAppeasementInValidInput_NoValue("Please select a coupon.", "Coupon Appeasement", out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddingAppeasementInValidInput_ForAppeasementPoints(string CouponName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Clicking on Submit With Out Entering Point: " + CouponName;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddingAppeasementInValidInput_NoValue("Please enter Points.", "Appeasement Points", out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddingAppeasementInValidInput_ForReward(string Reward, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Clicking on Submit With Out Selecting Reward: " + Reward;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddingAppeasementInValidInput_NoValue("Please select a reward.", "Reward Appeasement", out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddingAppeasementInValidInput_ForCouponCancel(string CouponName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Cancel Button and Select Coupon Message should be Removed";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddingAppeasementInValidInput_Cancel("Coupon Appeasement", out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddingAppeasementInValidInput_ForRewardCancel(string Reward, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Cancel Button and Select Reward Error Message should be Removed";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddingAppeasementInValidInput_Cancel("Coupon Appeasement", out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddingAppeasementInValidInput_ForPointsCancel(List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Cancel Button and Error Message should be Removed If any";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddingAppeasementInValidInput_Cancel("Appeasement Points", out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddCouponAppeasement(string CouponName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Adding Coupon Appeasement";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddCouponAppeasement(CouponName,out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddRewardAppeasement(string Reward, List<TestStep> listOfTestSteps)
        {
            string stepName = "Adding Reward Appeasement";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddRewardAppeasement(Reward, out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep AddRewardAppeasement_ForInActiveRule(string Reward, List<TestStep> listOfTestSteps)
        {
            string stepName = "Adding Reward Appeasement With InActive Rule";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddRewardAppeasement_ForInActiveRule(Reward, out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

    }
}
