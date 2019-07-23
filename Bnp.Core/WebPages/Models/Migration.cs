using BnPBaseFramework.Web;
using System.Linq;

namespace Bnp.Core.WebPages.Models
{
    public class Migration : ProjectBasePage
    {
        public Migration(DriverContext driverContext)
      : base(driverContext)
        { }
        public string MigrationCouponCategoryName => GetMigrationCategoryInfo("Migration_Coupon_Category", "Category Type");
        public string MigrationCouponame => GetMigrationCategoryInfo("Migration_Coupon_Category", "CreateCoupon");

        public string MigrationEnvironment => GetMigrationEnvironment("Migration", "MigrationEnvironment");
        public string MigrationOrderId => GetMigrationEnvironment("Migration", "MigrationOrderID");
        public string BuildMigrationSetName
        { get; set; }

        public enum MigrationSets
        {
            Migration_Coupon_Default,
            Migration_Coupon_IsGlobalflag,
            Migration_Coupon_PushNotifications,
            Migration_Product_AttributeSet,
            Migration_Promotion,
            Migration_EmailAWS,
            Migration_Email,
            Migration_ProductImage,
            Migration_Tier,
            Migration_Product,
            Migration_RuleWithPromotion,
            Migration_RewardWithLocationGroup,
            Migration_Website_Module,
            Migration_RewardWithPushNotification,
            Migration_Website_Set,
            Migration_Message_Set,
            Migration_MessageWithPushNotification_Set,
            Migration_Test,
            Migration_PushNotifications,
            Migration_GooglePay,
            Migration_ConvertedTextBlock,
            Migration_NonConvertedTextBlock,
            Migration_MTouchProcessingModule,
            Migration_VisitMap,
            Migration_RuleEvent,
            Migration_AttributeSet,
            Migration_Website_Module_Activation,
            Migration_ForgotPassword
        }

        public enum MigrationCheckBox
        {
            AttributeSets,
            DefaultRewards,
            Notifications

        }



        public enum MigrationStatus
        {
            Initiate,
            Approve,
            RunNow,
            Rollback
        }

        public enum Def
        {
            CouponDef,
            ContentAttributDef,
            Category
        }

    }
}
