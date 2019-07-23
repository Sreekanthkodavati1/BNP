using System.Linq;

namespace Bnp.Core.WebPages.Models
{
     public class AgentRegistration
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PasswordChangeRequired = false;

        public string Status { get; set; }
        
        public enum AgentStatus
        {
            Active,
            InActive,
            Locked
        }     
       
    }
}
