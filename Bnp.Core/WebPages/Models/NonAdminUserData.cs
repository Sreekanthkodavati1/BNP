
using BnPBaseFramework.Web;

namespace Bnp.Core.WebPages.Models
{
    public class NonAdminUserData : ProjectBasePage
    {
        public NonAdminUserData(DriverContext driverContext)
        : base(driverContext)
        { }

        public string AttributeAllContentType => GetNavigatorNodesInfo("GlobalCreateAttributSet", "AttributeSets");
        public string ChildAttributeSets => GetNavigatorNodesInfo("GlobalCreateAttributSet", "Attribute");
        public string ProductCategoryName => GetCategoryInfo("Product_Category", "Category Type");
        public string BonusCategoryName => GetCategoryInfo("Bonus_Category", "Category Type");
        public string CouponCategoryName => GetCategoryInfo("Coupon_Category", "Category Type");
        public string GlobalValueSet => GetCategoryInfo("Coupon_Category", "GlobalValueSet");
        public string ProductName => GetCategoryInfo("Product_Category", "Product");
        public string RewardName => GetCategoryInfo("Product_Category", "Reward");
        public string ProductVariantName => GetCategoryInfo("Product_Category", "Product_Variant");
        public string PartNumber => GetCategoryInfo("Product_Category", "PartNumber");
        public string Quantity => GetCategoryInfo("Product_Category", "Quantity");
        public string QuantityThreshold => GetCategoryInfo("Product_Category", "QuantityThreshold");
        public string VariantOrder => GetCategoryInfo("Product_Category", "VariantOrder");
        public string BonusName => GetCategoryInfo("Bonus_Category", "CreateBonus");
        public string CouponName => GetCategoryInfo("Coupon_Category", "CreateCoupon");
        public string CouponCode=> GetCategoryInfo("Coupon_Category", "CouponCode");
        public string CertNumberFormat => GetCategoryInfo("Coupon_Category", "CertNumberFormat");
        public static string MessageName => GetNavigatorNodesInfo("Message", "MessageName");
        public static string LocationGroupName => GetNavigatorNodesInfo("LocationGroup", "LocationGroupName");
        public static string ChannelName => GetNavigatorNodesInfo("Channels", "ChannelName");
        public static string MessageNameWithPushNotification => GetNavigatorNodesInfo("Message", "Migration_MessageNameWithPushNotification");
        public string DMCCode => GetNavigatorNodesInfo("email", "Email_DMCCode");
        public string EmailMessageName => GetNavigatorNodesInfo("email", "MailingName");
        public string SMSMessageName => GetNavigatorNodesInfo("sms", "SMSMessageName");
        public string SMSDMCCode => GetNavigatorNodesInfo("sms", "SMS_DMCCode");
        public string AttributeSets => GetNavigatorNodesInfo("CreateAttributSet", "AttributeSets");
        public string Attribute => GetNavigatorNodesInfo("CreateAttributSet", "Attribute");
        public string CSPortal_WebSiteName => GetWesiteName(0);
        public string MemberPortal_WebSiteName => GetWesiteName(1);
        public string PromotionName => GetNavigatorNodesInfo("Promotion", "PromotionName");
        public string ProductImageName => GetCategoryInfo("ProductImage", "ImageFileName");
        public string ProductImageOrder => GetCategoryInfo("ProductImage", "ImageOrder");
        public string MigrationAttributeAllContentType => GetNavigatorNodesInfo("GlobalCreateAttributSet", "MigrationAttributeSets");
        public string AWSEmailTemplate => GetNavigatorNodesInfo("email_aws", "Template");
        public string AWSEmailName => GetNavigatorNodesInfo("email_aws", "Email");
        public string Subject => GetNavigatorNodesInfo("email_aws", "Subject");
        public string FromMail => GetNavigatorNodesInfo("email_aws", "FromEmail");
        public string Default_Notification => GetNavigatorNodesInfo("notification", "default_notification");
        public string Migrate_Notification => GetNavigatorNodesInfo("notification", "migration_notification");
        public string JobName => GetCategoryInfo("ScheduleJob", "Job");
        public string StoreName => GetNavigatorNodesInfo("Store", "StoreName");
        public string MigrationPromotionName => GetNavigatorNodesInfo("Promotion", "MigrationPromotionName");
        public string GlobalValue_Key => GetNavigatorNodesInfo("global_value", "Key");
        public string GlobalValue_Value => GetNavigatorNodesInfo("global_value", "Value");
        public string Default_TextBlock => GetNavigatorNodesInfo("TextBlock", "Default_TextBlock");
        public string Migration_TextBlock => GetNavigatorNodesInfo("TextBlock", "Migration_TextBlock");
        public string Default_GPay => GetNavigatorNodesInfo("GooglePay", "default_googlePay");
        public string Migrate_GPay => GetNavigatorNodesInfo("GooglePay", "migration_googlePay");
        public string Migration_ConvertedTextBlock => GetNavigatorNodesInfo("TextBlock", "Migration_ConvertedTextBlock");
        public string Migration_NonConvertedTextBlock => GetNavigatorNodesInfo("TextBlock", "Migration_NonConvertedTextBlock");
        public string Converted_Text => GetNavigatorNodesInfo("TextBlock", "Converted_Text");
        public string NonConverted_Text => GetNavigatorNodesInfo("TextBlock", "NonConverted_Text");
    }
}
