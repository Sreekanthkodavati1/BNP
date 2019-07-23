using BnPBaseFramework.Web;
using System.Configuration;
using System.Linq;

namespace Bnp.Core.WebPages.Models
{
    public class NavigatorUsers:ProjectBasePage
    {
        public NavigatorUsers(DriverContext driverContext)
          : base(driverContext)
        { }
        #region Naviator Users read from config file      
        public static string bpAdminUser => GetUserInfo("bpadmin", "username");
        public static string bpAdminUser_Password => GetUserInfo("bpadmin", "password");

        public static string AdminUser => GetUserInfo("LWAdminUser", "username");
        public static string AdminUser_Password => GetUserInfo("LWAdminUser", "password");

        public static string KEYUser => GetUserInfo("KeyAdminUser", "username");
        public static string KEYUser_Password => GetUserInfo("KeyAdminUser", "password");

        public static string DBAUser => GetUserInfo("DBAAdminUser", "username");
        public static string DBAUser_Password => GetUserInfo("DBAAdminUser", "password");

        public static string WebUser => GetUserInfo("WebAdminUser", "username");
        public static string WebUser_Password => GetUserInfo("WebAdminUser", "password");

        public static string NonAdminUser => GetUserInfo("UserWithAllRoles", "username");
        public static string NonAdminUser_Password => GetUserInfo("UserWithAllRoles", "password");

        public static string NavigatorPassword => GetUserInfo("UserWithAllRoles", "password");

        public static string RegressionUser => GetUserInfo("UserWithAllRoles", "username");
        public static string RegressionPassword => GetUserInfo("UserWithAllRoles", "username");
        #endregion
    }
}
