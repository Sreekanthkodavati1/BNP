namespace Bnp.Core.WebPages.Models
{
    public class CsPortalData
    {
        public static string csadmin => ProjectBasePage.GetCsportalTestDataInfo("CsAdminuser", "csadmin");
        public static string csadmin_password => ProjectBasePage.GetCsportalTestDataInfo("CsAdminuser", "csadmin_password");
        public static string BTA_DEV_CS_LogPath => ProjectBasePage.GetCsportalTestDataInfo("CsAdminuser", "BTA_DEV_CS_LogPath");

    }
}
