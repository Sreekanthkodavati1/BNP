using BnPBaseFramework.Web;
using System.Data;
using System.Linq;

namespace Bnp.Core.WebPages.Models
{

    public class Rules
    {

        public string RuleName { get; set; }
        public string RuleOwner { get; set; }
        public string RuleType { get; set; }
        public string Invocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Sequence { get; set; }
        public string Expression { get; set; }
        public bool Targeted { get; set; }
        public string Promotion { get; set; }
        public bool AlwaysRun { get; set; }
        public bool ContinueOnError { get; set; }
        public bool LogExecution { get; set; }
        public bool QueueruleExecution { get; set; }

        public bool Rulestatus_ToInactive = false;

    }
    public class Rules_Configurations_CreateVirtual_Card
    {

        public bool Skipcreation_Onexistingvirtualcard { get; set; }
        public string LoyaltyId_GenerationSource { get; set; }
        public string RuleVersion { get; set; }
        public string RuleDescription { get; set; }

    }

    public class Rules_IssueRewardfromcatalog
    {
        public string FulfillmentMethod { get; set; }
        public string PointsConsumedWhenIssued { get; set; }
        public string ExpirationDate { get; set; }
        public string RewardIssuedEmail { get; set; }
        public bool AssignLoyaltyWareCertificate { get; set; }

    }
    public class Rules_AwardLoyaltyCurrency
    {
        public string AccrualExpression { get; set; }
        public string TierEvaluationRule { get; set; }
        public bool AllowZeroPointAward	 { get; set; }

    }
    public class Rules_IssueCoupon
    {
        public bool AssignLoyaltyWareCertificate { get; set; }
        public string CouponName { get; set; }

    }
    public class Rules_IssueReward
    {
        public string FulfillmentMethod { get; set; }
        public string RewardTypePoints { get; set; }
        public string PointsConsumedWhenIssued { get; set; }
        public bool AssignLoyaltyWareCertificate { get; set; }
        public bool IssuetheMembersRewardChoice { get; set; }
        public string RewardName { get; set; }


    }

    public class Rules_IssueMessage
    {
        public bool AllowMultipleMessages { get; set; }
    }
    public class Rules_EvaluateTier
    {
        public string VirtualCardLocationLogic	 { get; set; }
        public bool Tiers_Standard { get; set; }
        public bool Tiers_Silver { get; set; }
        public bool Tiers_Gold { get; set; }
        public bool Tiers_Platinum { get; set; }

    }
    public class Rules_CFContactUsEmailRule
    {
        public string TheEmailoftherecipient { get; set; }
        public string Triggered_Email { get; set; }
        public string Rule_Version { get; set; }

    }
}