using BnPBaseFramework.Web;
using System.Configuration;

namespace Bnp.Core.WebPages.Models
{
    class FrameworkConfigData : ProjectBasePage
    {
        public FrameworkConfigData(DriverContext driverContext)
         : base(driverContext)
        { }

        #region Framework Config read from config file      
        public static string DmcUrl => GetNavigatorNodesInfo("FrameworkConfig", "DmcUrl");
        public static string DmcUsername => GetNavigatorNodesInfo("FrameworkConfig", "DmcUsername");
        public static string DmcPassword => GetNavigatorNodesInfo("FrameworkConfig", "DmcPassword");
        public static string LWEmailProvider => GetNavigatorNodesInfo("FrameworkConfig", "LWEmailProvider"); 
        public static string LoyaltyCurrencyAsPayment => GetNavigatorNodesInfo("FrameworkConfig", "LoyaltyCurrencyAsPayment DefaultCurrency");   
        #endregion
    }
}
