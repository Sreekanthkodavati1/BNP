namespace Bnp.Core.WebPages.Models
{
    public class Users
    {


        public enum AdminRole
        {
            DBA,
            KEY,
            LWADM,
            WEB,
            USER
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ConfirmPassword { get; set; }

    }

}
