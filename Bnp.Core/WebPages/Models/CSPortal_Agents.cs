using System.Configuration;

namespace Bnp.Core.WebPages.Models
{
    public class AgentValues
    {
        public static string AdminAgent => ProjectBasePage.GetCsportalTestDataInfo("Agents", "AdminAgent");
        public static string SrAdminAgent => ProjectBasePage.GetCsportalTestDataInfo("Agents", "SrAdminAgent");
        public static string JrAdminAgent => ProjectBasePage.GetCsportalTestDataInfo("Agents", "JrAdminAgent"); 
        public static string Agentpassword => ProjectBasePage.GetCsportalTestDataInfo("Agents", "Agent_password");
        public static string ChangePasswordTestAgent => ProjectBasePage.GetCsportalTestDataInfo("Agents", "ChangePasswordTestAgent");
        public static string ForgotPasswordTestAgent => ProjectBasePage.GetCsportalTestDataInfo("Agents", "ForgotPasswordTestAgent");
    }
}
