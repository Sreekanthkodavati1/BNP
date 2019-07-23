using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Bnp.Core.WebPages.MemberPortal
{
    public class MemberPortal_MyWalletPage : ProjectBasePage
    {
        /// <summary>
        /// This class handles Member Portal >My Wallet Page
        /// </summary>
        public MemberPortal_MyWalletPage(DriverContext driverContext)
      : base(driverContext) { }

        #region My Wallet Page Locators
        //My Loyalty Cards locators
        private readonly ElementLocator Header_MyLoyaltyCards = MemberProfile_Header_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards));
        private readonly ElementLocator Section_LoyaltyCards_Total = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), MyLoyaltyCardsSections.Total.ToString());
        private readonly ElementLocator Section_LoyaltyCards_LoyaltyIDNumber = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), EnumUtils.GetDescription(MyLoyaltyCardsSections.LoyaltyIDNumber));
        private readonly ElementLocator Section_LoyaltyCards_Status = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), MyLoyaltyCardsSections.Status.ToString());
        private readonly ElementLocator Section_LoyaltyCards_DateRegistered = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), EnumUtils.GetDescription(MyLoyaltyCardsSections.DateRegistered));
        private readonly ElementLocator Section_LoyaltyCards_CardType = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), EnumUtils.GetDescription(MyLoyaltyCardsSections.CardType));
        private readonly ElementLocator Button_MyLoyaltyCards_View = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), MyWalletButtons.View.ToString());
        private readonly ElementLocator Button_MyLoyaltyCards_SendToMyWallet = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), EnumUtils.GetDescription(MyWalletButtons.SendToAppleWallet));
        private readonly ElementLocator Button_MyLoyaltyCards_Print = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), MyWalletButtons.Print.ToString());
        private readonly ElementLocator Button_MyLoyaltyCards_Done = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), MyWalletButtons.Done.ToString());
        private readonly ElementLocator Label_LoyalytID = new ElementLocator(Locator.XPath, "//span[text()='Loyalty ID Number']//following-sibling::span");
        //My Coupons Locators
        private readonly ElementLocator Header_MyCoupons = MemberProfile_Header_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons));
        private readonly ElementLocator Section_MyCoupons_Total = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons), MyCouponsSections.Total.ToString());
        private readonly ElementLocator Section_Coupons_ShortDescription = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons), EnumUtils.GetDescription(MyCouponsSections.ShortDescription));
        private readonly ElementLocator Section_MyCoupons_TimesUsed = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons), EnumUtils.GetDescription(MyCouponsSections.TimesUsed));
        private readonly ElementLocator Section_MyCoupons_UsesLeft = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons), EnumUtils.GetDescription(MyCouponsSections.UsesLeft));
        private readonly ElementLocator Button_MyCoupons_Print = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons), MyWalletButtons.Print.ToString());
        private readonly ElementLocator Button_MyCoupons_Done = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons), MyWalletButtons.Done.ToString());
        private readonly ElementLocator Button_MyCoupons_View = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyCoupons), MyWalletButtons.View.ToString());
        private readonly ElementLocator Button_MyCoupons_SendToMyWallet = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyLoyaltyCards), EnumUtils.GetDescription(MyWalletButtons.SendToAppleWallet));
        //My Reward Locators
        private readonly ElementLocator Header_MyRewards = MemberProfile_Header_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards));
        private readonly ElementLocator Section_MyRewards_Total = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), MyRewardsSections.Total.ToString());
        private readonly ElementLocator Section_MyRewards_DateAwarded = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), EnumUtils.GetDescription(MyRewardsSections.DateAwarded));
        private readonly ElementLocator Section_MyRewards_Expiration = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), EnumUtils.GetDescription(MyRewardsSections.Expiration));
        private readonly ElementLocator Section_MyRewards_OrderStatus = MemberProfile_Section_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), EnumUtils.GetDescription(MyRewardsSections.OrderStatus));
        private readonly ElementLocator Button_MyRewards_Print = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), MyWalletButtons.Print.ToString());
        private readonly ElementLocator Button_MyRewards_Done = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), MyWalletButtons.Done.ToString());
        private readonly ElementLocator Button_MyRewards_View = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), MyWalletButtons.View.ToString());
        private readonly ElementLocator Button_RewardFaceBookShare = new ElementLocator(Locator.XPath, "//a[text()='My Rewards']//following::div[@class='social-share']//div[@class='fb-share-button fb_iframe_widget']");
        private readonly ElementLocator Button_RewardTwitterShare = new ElementLocator(Locator.XPath, "//a[text()='My Rewards']//following::div[@class='social-share']//following::iframe[@id='twitter-widget-0']");
        private readonly ElementLocator Button_RewardGooglePlusShare = new ElementLocator(Locator.XPath, "//a[text()='My Rewards']//following::div[@class='social-share']//div[@id='___plus_0']");
        private readonly ElementLocator Textbox_RewardFromDate = MemberPortal_Textbox_Custom_ElementLocatorXpath("From Date: ");
        private readonly ElementLocator Textbox_RewardToDate = MemberPortal_Textbox_Custom_ElementLocatorXpath("To Date: ");
        private readonly ElementLocator Button_MyRewards_Search = MemberProfile_Button_Custom_ElementLocatorXpath(EnumUtils.GetDescription(MyWalletSections.MyRewards), "Search");
        private readonly ElementLocator Select_RewardStatus = Reward_Select_Custom_ElementLocatorXpath("Reward Status:");
        private readonly ElementLocator Select_RewardName = Reward_Select_Custom_ElementLocatorXpath("Reward:");

        #endregion

        public enum MyWalletSections
        {
            [DescriptionAttribute("My Rewards")]
            MyRewards,
            [DescriptionAttribute("My Coupons")]
            MyCoupons,
            [DescriptionAttribute("My Loyalty Cards")]
            MyLoyaltyCards
        }

        public enum MyLoyaltyCardsSections
        {
            Total,
            [DescriptionAttribute("Loyalty ID Number")]
            LoyaltyIDNumber,
            Status,
            [DescriptionAttribute("Date Registered")]
            DateRegistered,
            [DescriptionAttribute("Card Type")]
            CardType
        }

        public enum MyCouponsSections
        {
            Total,
            [DescriptionAttribute("Coupon Name")]
            CouponName,
            [DescriptionAttribute("Short Description:")]
            ShortDescription,
            [DescriptionAttribute("Times Used:")]
            TimesUsed,
            [DescriptionAttribute("Uses Left:")]
            UsesLeft
        }

        public enum MyWalletButtons
        {
            [DescriptionAttribute("Send To Apple Wallet")]
            SendToAppleWallet,
            Search,
            View,
            Print,
            Done,
            First,
            Previous,
            Next,
            Last
        }
        public enum MyRewardsSections
        {
            Total,
            [DescriptionAttribute("Date Awarded")]
            DateAwarded,
            [DescriptionAttribute("Expiration")]
            Expiration,
            [DescriptionAttribute("Order Status")]
            OrderStatus,
        }

        public enum RewardStatus
        {
            Active,
            Expired
        }

        /// <summary>
        /// Method to verify My vallet page
        /// </summary>
        /// <param name="strStatus"> return string status</param>
        /// <returns>True if page verified successfully, else throws exception</returns>
        public bool VerifyMyWalletPage(out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Header_MyRewards, .5) && Driver.IsElementPresent(Header_MyCoupons, .5) && Driver.IsElementPresent(Header_MyLoyaltyCards, .5))
                {
                    strStatus = "Successfully verified My Wallet page";
                    return true;
                }
                else
                {
                    throw new Exception("Failed to verify My Wallet page");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify My Wallet page");
            }
        }

        /// <summary>
        /// Method to verify the Loyalty ID
        /// </summary>
        /// <param name="loyaltyID">Loyalty ID</param>
        /// <param name="status"> String status of Loyalty ID match or failed to match</param>
        /// <returns>Returns true if IDs match, else throws exception</returns>
        public bool VerifyMyLoyaltyCardsLoyaltyID(string loyaltyID, out string status)
        {
            try
            {
                Driver.ScrollIntoMiddle(Label_LoyalytID);
                if (Driver.GetElement(Label_LoyalytID).GetTextContent().Equals(loyaltyID))
                {
                    status = loyaltyID + " Loyalty ID matches";
                    return true;
                }
                else
                {
                    throw new Exception("Failed to  match Loyalty ID: " + loyaltyID);
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to  match Loyalty ID: " + loyaltyID);
            }
        }

        /// <summary>
        /// Method to verify Loyalty Cards section
        /// </summary>
        /// <param name="strStatus"> return string status</param>
        /// <returns>True if Loyalty Cards section verified successfully, else throws exception</returns>
        public bool VerifyLoyaltyCardsSection(out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Section_LoyaltyCards_Total, .5))
                {
                    if (Driver.IsElementPresent(Section_LoyaltyCards_LoyaltyIDNumber, .5) && Driver.IsElementPresent(Section_LoyaltyCards_Status, .5) && Driver.IsElementPresent(Section_LoyaltyCards_DateRegistered, .5) && Driver.IsElementPresent(Section_LoyaltyCards_CardType, .5))
                    {
                        strStatus = "Successfully verified My Loyalty Cards sections";
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify My Loyalty Cards sections");
            }
            throw new Exception("Failed to verify My Loyalty Cards sections");
        }

        /// <summary>
        /// Method to verify button options on My Loyalty Cards section
        /// </summary>
        /// <param name="strStatus">String status of verification</param>
        /// <returns>
        /// Returns true if button verification successful, else throws exception
        /// </returns>
        public bool VerifyLoyaltyCardsButtonOptions(out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Section_LoyaltyCards_Total, .5))
                {
                    if (Driver.IsElementPresent(Button_MyLoyaltyCards_View, .5) && Driver.IsElementPresent(Button_MyLoyaltyCards_SendToMyWallet, .5))
                    {
                        Click_OnButton(Button_MyLoyaltyCards_View);
                        if (Driver.IsElementPresent(Button_MyLoyaltyCards_Done, .5) && Driver.IsElementPresent(Button_MyLoyaltyCards_Print, .5))
                        {
                            Click_OnButton(Button_MyLoyaltyCards_Done);
                            strStatus = "Successfully verified My Loyalty Cards button options(VIEW,Send To My Wallet, PRINT and DONE)";
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify My Loyalty Cards View options");
            }
            throw new Exception("Failed to verify My Loyalty Cards button options(VIEW,Send To My Wallet, PRINT and DONE");
        }

        /// <summary>
        /// Method to verify Coupons section
        /// </summary>
        /// <param name="strStatus"> return string status</param>
        /// <returns>True if Coupons section verified successfully, else throws exception</returns>
        public bool VerifyMyCouponSection(string CouponName, out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Section_MyCoupons_Total, .5))
                {
                    ElementLocator PageCount = new ElementLocator(Locator.XPath, "//div[@id='CouponsList']//div[@class='section table_section']//div[@class='section_content']//div[@class='pager']//a[contains(text(),'Next')]//preceding::a[1]");
                    ElementLocator Page = new ElementLocator(Locator.XPath, "//div[@id='CouponsList']//div[@class='section table_section']//div[@class='section_content']//div[@class='pager']//span//a[contains(text(),'2')]");
                    if (Driver.IsElementPresent(Page, .5))
                    {
                        var NumberOfPages = Driver.GetElement(PageCount).Text;
                        int TotalPages = Convert.ToInt32(NumberOfPages);
                        for (var i = 1; i <= TotalPages; i++)
                        {
                            if (i > 1)
                            {
                                Driver.FindElement(By.XPath("//div[@id='CouponsList']//div[@class='section table_section']//div[@class='section_content']//div[@class='pager']//span//a[contains(text(),'"+ i +"')]")).ClickElement();
                                string couponName = "//span[contains(text(),'" + CouponName + "')]";
                                if (Driver.IsElementPresent(By.XPath(couponName))
                                    && Driver.IsElementPresent(Section_Coupons_ShortDescription, .5)
                                    && Driver.IsElementPresent(Section_MyCoupons_TimesUsed, .5)
                                    && Driver.IsElementPresent(Section_MyCoupons_UsesLeft, .5))
                                {
                                    strStatus = "Successfully verified My Coupons section  ;Available Coupon Details:"+CouponName;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        string couponName = "//span[contains(text(),'" + CouponName + "')]";
                        if (Driver.IsElementPresent(By.XPath(couponName))
                            && Driver.IsElementPresent(Section_Coupons_ShortDescription, .5)
                            && Driver.IsElementPresent(Section_MyCoupons_TimesUsed, .5)
                            && Driver.IsElementPresent(Section_MyCoupons_UsesLeft, .5))
                        {
                            strStatus = "Successfully verified My Coupons section  ;Available Coupon Details:" + CouponName;
                            return true;
                        }
                    }
                       
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify My Coupons section with Coupon Name" + CouponName);
            }
            throw new Exception("Failed to verify My Coupons section with Coupon Name" + CouponName);
        }

        /// <summary>
        /// Method to verify button options on My Coupons section
        /// </summary>
        /// <param name="strStatus">String status of verification<</param>
        /// <returns>
        /// Returns true if button verification successful, else throws exception
        /// </returns>
        public bool VerifyMyCouponsButtonOptions(out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Section_MyCoupons_Total, .5))
                {
                    if (Driver.IsElementPresent(Button_MyCoupons_View, .5) && Driver.IsElementPresent(Button_MyCoupons_SendToMyWallet, .5))
                    {
                        Click_OnButton(Button_MyCoupons_View);
                        if (Driver.IsElementPresent(Button_MyCoupons_Done, .5) && Driver.IsElementPresent(Button_MyCoupons_Print, .5))
                        {
                            if (VerifyShareButtonOptions(EnumUtils.GetDescription(MyWalletSections.MyCoupons)))
                            {
                                Click_OnButton(Button_MyCoupons_Done);
                                strStatus = "Successfully verified My Coupons button options(VIEW,Send To My Wallet,PRINT,DONE and Share Button Options";
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify My Coupons button options(VIEW,Send To My Wallet,PRINT,DONE and Share Button Options");
            }
            throw new Exception("Failed to verify My Coupons button options(VIEW,Send To My Wallet,PRINT,DONE and Share Button Options");
        }

        /// <summary>
        /// Method to verify Reward section
        /// </summary>
        /// <param name="strStatus"> return string status</param>
        /// <returns>True if Reward section verified successfully, else throws exception</returns>
        public bool VerifyMyRewardSection(string rewardName, string fromDate, string toDate, string rewardStatus, out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Section_MyRewards_Total, .5))
                {
                    SelectElement_AndSelectByText(Select_RewardStatus, rewardStatus);
                    SelectElement_AndSelectByText(Select_RewardName, rewardName);
                    Driver.GetElement(Textbox_RewardFromDate).SendText(fromDate);
                    Driver.GetElement(Textbox_RewardToDate).SendText(toDate);
                    Click_OnButton(Button_MyRewards_Search);
                    ElementLocator PageCount = new ElementLocator(Locator.XPath, "//div[@class='section_content']//div[@class='pager'][1]//span[1]//a[@class='btn btn-sm'][contains(text(),'Next')]//preceding::a[1]");
                    ElementLocator Page = new ElementLocator(Locator.XPath, "//a[contains(text(),'2')]");
                    if (Driver.IsElementPresent(Page, .5))
                    {
                        var NumberOfPages = Driver.GetElement(PageCount).Text;
                        int TotalPages = Convert.ToInt32(NumberOfPages);
                        for (var i = 1; i <= TotalPages; i++)
                        {
                            if (i > 1)
                            {
                                Driver.FindElement(By.XPath("//a[contains(text(),'" + i + "')]")).ClickElement();
                                string RewardName = "//span[contains(text(),'" + rewardName + "')]";
                                if (Driver.IsElementPresent(By.XPath(RewardName))
                                    && Driver.IsElementPresent(Section_MyRewards_DateAwarded, .5)
                                    && Driver.IsElementPresent(Section_MyRewards_Expiration, .5)
                                    && Driver.IsElementPresent(Section_MyRewards_OrderStatus, .5))
                                {
                                    Driver.ScrollIntoMiddle(Section_MyRewards_DateAwarded);
                                    strStatus = "Successfully verified My Reward section and Reward Name; Reward details are:" + rewardName; return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        string RewardName = "//span[contains(text(),'" + rewardName + "')]";
                        if (Driver.IsElementPresent(By.XPath(RewardName))
                            && Driver.IsElementPresent(Section_MyRewards_DateAwarded, .5)
                            && Driver.IsElementPresent(Section_MyRewards_Expiration, .5)
                            && Driver.IsElementPresent(Section_MyRewards_OrderStatus, .5))
                        {
                            Driver.ScrollIntoMiddle(Section_MyRewards_DateAwarded);
                            strStatus = "Successfully verified My Reward section and Reward Name; Reward details are:"+rewardName;return true;
                        }
                        else
                            throw new Exception("Failed to verify My Reward section with RewardName Name" + rewardName);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify My Reward sections");
            }
            throw new Exception("Failed to verify My Reward sections");
        }

        /// <summary>
        /// Method to verify button options on My Reward section
        /// </summary>
        /// <param name="strStatus">String status of verification<</param>
        /// <returns>
        /// Returns true if button verification successful, else throws exception
        /// </returns>
        public bool VerifyMyRewardButtonOptions(out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Section_MyRewards_Total, .5))
                {
                    if (Driver.IsElementPresent(Button_MyRewards_View, .5))
                    {
                        Click_OnButton(Button_MyRewards_View);
                        if (Driver.IsElementPresent(Button_MyRewards_Done, .5) && Driver.IsElementPresent(Button_MyRewards_Print, .5))
                        {
                            if (VerifyShareButtonOptions(EnumUtils.GetDescription(MyWalletSections.MyRewards)))
                            {
                                Click_OnButton(Button_MyRewards_Done);
                                Driver.ScrollIntoMiddle(Button_MyRewards_View);
                                strStatus = "Successfully verified My Reward View options, With Done and Print Options";
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify My Reward View options");
            }
            throw new Exception("Failed to verify My Reward View options");
        }

        /// <summary>
        /// Method to verify social media share button options in My Coupons section
        /// </summary>
        /// <param name="SectionName">String status of verification<</param>
        /// <returns>
        /// Returns true if share button options verification successful, else throws an exception
        /// </returns>     
        public bool VerifyShareButtonOptions(string SectionName)
        {
            try
            {
                ElementLocator Button_FaceBookShare = new ElementLocator(Locator.XPath, "//a[text()='"+ SectionName + "']//following::div[@class='social-share']//div[@class='fb-share-button fb_iframe_widget']");
                ElementLocator Button_TwitterShare = new ElementLocator(Locator.XPath, "//a[text()='" + SectionName + "']//following::div[@class='social-share']//following::iframe[@id='twitter-widget-0']");
                ElementLocator Button_GooglePlusShare = new ElementLocator(Locator.XPath, "//a[text()='" + SectionName + "']//following::div[@class='social-share']//div[@id='___plus_0']");
                if (Driver.IsElementPresent(Button_FaceBookShare, 2)
                    && Driver.IsElementPresent(Button_TwitterShare,2)
                    && Driver.IsElementPresent(Button_GooglePlusShare, 2))
                {
                    Driver.ScrollIntoMiddle(Button_FaceBookShare);
                    return true;
                }
                else
                    throw new Exception("Failed to verify Share Button options(Facebook, Twitter, Google+)");
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify Share Button options(Facebook, Twitter, Google+)");
            }
        }

        /// <summary>
        /// Method for verifying the social media options on rewards and coupons
        /// </summary>
        /// <param name="rewardOrCoupon">Type rewards or coupons</param>
        /// <param name="strStatus">Status returing successfull verification else throws exception</param>
        /// <returns>true on successfull verification else throws exception</returns>
        public bool VerifySociaMediaOptionsForRewardsAndCoupons(string rewardOrCoupon, out string strStatus)
        {
            try
            {
                if(rewardOrCoupon.Equals("rewards"))
                {
                    if (Driver.IsElementPresent(Section_MyRewards_Total, .5))
                    {
                        if (Driver.IsElementPresent(Button_MyRewards_View, .5))
                        {
                            Click_OnButton(Button_MyRewards_View);
                            if (Driver.IsElementPresent(Button_MyRewards_Done, .5) && Driver.IsElementPresent(Button_MyRewards_Print, .5))
                            {
                                if (VerifyShareButtonOptions(EnumUtils.GetDescription(MyWalletSections.MyRewards)))
                                {
                                    Thread.Sleep(2000);//Adding wait to load all social media elements
                                    strStatus = "Successfully verified My Rewards Social Media options-Facebook,Twitter and Google Plus";
                                    return true;
                                }
                            }
                        }
                    }
                }
                else if (rewardOrCoupon.Equals("coupons"))
                {
                    if (Driver.IsElementPresent(Section_MyCoupons_Total, .5))
                    {
                        if (Driver.IsElementPresent(Button_MyCoupons_View, .5))
                        {
                            Click_OnButton(Button_MyCoupons_View);
                            if (Driver.IsElementPresent(Button_MyCoupons_Done, .5) && Driver.IsElementPresent(Button_MyCoupons_Print, .5))
                            {
                                if (VerifyShareButtonOptions(EnumUtils.GetDescription(MyWalletSections.MyCoupons)))
                                {
                                    Thread.Sleep(2000);//Adding wait to load all social media elements
                                    strStatus = "Successfully verified My Coupons Social Media Options-Facebook,Twitter and Google Plus";
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify Social Media-Facebook,Twitter and Google Plus Options for "+rewardOrCoupon);
            }
            throw new Exception("Failed to verify Social Media-Facebook,Twitter and Google Plus Options for " + rewardOrCoupon);
        }
    }
}