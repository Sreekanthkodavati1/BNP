using System.ComponentModel;

namespace Bnp.Core.WebPages.Models
{
    public class CategoryFields
    {
        public string ID { get; set; }
        public string RewardTypeValue { get; set; }
        public string CategoryTypeValue { get; set; }
        public string Name { get; set; }
        public string ChannelType { get; set; }
        public string LocationGroupName { get; set; }
        public string LocationGroup { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string ExpiryDate { get; set; }
        public string CategoryName { get; set; }
        public string SetType { get; set; }
        public string ValueToSetInAttribute { get; set; }
        public string Logo_Img_Hero { get; set; }
        public string CouponCode { get; set; }
        public string UsesAllowed { get; set; }
        public string IsGlobal { get; set; }
        public string UsesAllowedDynamic => ProjectBasePage.GetCategoryInfo("CouponMaxUses", "UsesAllowed");
        public string MaxUsesPerMonth => ProjectBasePage.GetCategoryInfo("CouponMaxUses", "PerMonth");
        public string MaxUsesPerYear => ProjectBasePage.GetCategoryInfo("CouponMaxUses", "PerYear");
        public string MaxUsesPerWeek => ProjectBasePage.GetCategoryInfo("CouponMaxUses", "PerWeek");
        public string MaxUsesPerDay => ProjectBasePage.GetCategoryInfo("CouponMaxUses", "PerDay");
        public string MaxUsesPerMonthAllCombination => ProjectBasePage.GetCategoryInfo("CouponMaxUsesWithAllIntervalCombination", "PerMonth");
        public string MaxUsesPerYearAllCombination => ProjectBasePage.GetCategoryInfo("CouponMaxUsesWithAllIntervalCombination", "PerYear");
        public string MaxUsesPerWeekAllCombination => ProjectBasePage.GetCategoryInfo("CouponMaxUsesWithAllIntervalCombination", "PerWeek");
        public string MaxUsesPerDayAllCombination => ProjectBasePage.GetCategoryInfo("CouponMaxUsesWithAllIntervalCombination", "PerDay");
        public string MultiLanguage { get; set; }
        public string ChannelProperties { get; set; }
        public string BalanceNeeded { get; set; }
        public string AttributeName { get; set; }
        public string PredicateDropDown { get; set; }
        public string ProductQuantity => ProjectBasePage.GetCategoryInfo("Product", "Quantity");
        public string Quantity{ get; set; }
        public string ConversionRate { get; set; }
        public string PartNumber { get; set; }
        public string QuantityThreshold { get; set; }
        public string VariantOrder { get; set; }
        public string RewardBalanceNeeded => ProjectBasePage.GetCategoryInfo("Product_Category", "BalanceNeeded");
        public string CertificateTypeCode { get; set; }
        public string TierTypeValue { get; set; }
        public string BasePrice { get; set; }
        public string ContentType { get; set; }
        public string DataType { get; set; }
        public string DefaultValues { get; set; }
        public string ContentAttributeName { get; set; }
        public string ProductBasePrice => ProjectBasePage.GetCategoryInfo("Product", "BasePrice");
        public string ProductQuantityThreshold => ProjectBasePage.GetCategoryInfo("Product", "QuantityThreshold");
        public string ExchangeRateValue => ProjectBasePage.GetCategoryInfo("ExchangeRates", "ExchangeRateValue");
        public string DefaultCurrencyValue => ProjectBasePage.GetCategoryInfo("ExchangeRates", "DefaultCurrencyValue");
        public string ToCurrencyName => ProjectBasePage.GetCategoryInfo("ExchangeRates", "ToCurrencyName");
        public string FromCurrencyName => ProjectBasePage.GetCategoryInfo("ExchangeRates", "FromCurrencyName");
        public string PushNotification { get; set; }
        public string FrequencyDropDown { get; set; }
        public string Hour { get; set; }
        public string RunEveryHour => ProjectBasePage.GetCategoryInfo("ScheduleJob", "Hour");
        public string CategoryDescription => ProjectBasePage.GetCategoryInfo("Product_Category", "Description");

        public enum Property
        {
            Name,
            Active,
            Category,
            [DescriptionAttribute("Coupon Code")]
            CouponCode,
            AutoAttributeSet,
            Bonus
        }
        public enum CategoryType
        {
            Product,
            Bonus,
            Coupon
        }
        public enum Languages
        {
            English,
            French,
            Spanish,
            Russian,
            German,
            Japanese
        }
        public enum Channel
        {
            Web,
            Mobile,
            SMS,
            Email,
            Print,
            Push
        }
        public enum Predicates
        {
            Eq,
            Ne,
            Le,
            Lt,
            Ge,
            Gt,
            Like
        }

        public enum RewardType
        {
            Regular,
            Payment
        }

        public enum TierType
        {
            Standard,
            Silver,
            Gold,
            Platinum,
            Tier_Defaults,
            Tier_Multiple,
            Tier_Migration
        }
         
        public enum ExchangeRatePropertyType
        {
            FromCurrency,
            ToCurrency,
            FromCurrencyName,
            ToCurrencyName
        }

        public enum FrequencyType
        {
            Minute,
            Hourly,
            Daily,
            OneTime
        }
    }
}