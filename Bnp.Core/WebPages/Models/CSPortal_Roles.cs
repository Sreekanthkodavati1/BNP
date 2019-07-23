namespace Bnp.Core.WebPages.Models
{
    public class RoleValue
    {
        public static string Admin => ProjectBasePage.GetCsportalTestDataInfo("Roles", "AdminRole");
        public static string SrAdmin => ProjectBasePage.GetCsportalTestDataInfo("Roles", "SrAdminRole");
        public static string JrAdmin => ProjectBasePage.GetCsportalTestDataInfo("Roles", "JrAdminRole");
        public static string AdminRole_PointAwardLimit => ProjectBasePage.GetCsportalTestDataInfo("Roles", "AdminRole_PointAwardLimit");
        public static string SrAdminRole_PointAwardLimit => ProjectBasePage.GetCsportalTestDataInfo("Roles", "SrAdminRole_PointAwardLimit");
        public static string JrAdminRole_PointAwardLimit => ProjectBasePage.GetCsportalTestDataInfo("Roles", "JrAdminRole_PointAwardLimit");
    }
}
