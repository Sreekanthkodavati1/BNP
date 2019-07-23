using System.Configuration;

namespace Bnp.Core.WebPages.Models
{
    public class Login
    {
        public  string UserName { get; set; }
        public string Password { get; set; }

        public string Url => ProjectBasePage.GetApplicationUrls(0);
        public string Csp_url => ProjectBasePage.GetApplicationUrls(1);
        public string MemberPortal_url => ProjectBasePage.GetApplicationUrls(2);
    }
}
