using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.WebPages.Models
{
    public class MemberPortalData
    {
        public static string MP_username => ProjectBasePage.GetMemberPortalTestDataInfo("MP_username");
        public static string MP_password => ProjectBasePage.GetMemberPortalTestDataInfo("MP_password");
        public static string RewardName => ProjectBasePage.GetMemberPortalTestDataInfo("RewardName");
        public static string ForgotPasswordTestMember => ProjectBasePage.GetMemberPortalTestDataInfo("ForgotPasswordTestMember");
        public static string BTA_DEV_MP_LogPath => ProjectBasePage.GetMemberPortalTestDataInfo("BTA_DEV_MP_LogPath");
    }
}
