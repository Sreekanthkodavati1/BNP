using Bnp.Core.Tests.API.Validators;
using BnPBaseFramework.Web;
using RandomNameGenerator;

namespace Bnp.Core.WebPages.Models
{
    /// <summary>
    /// Model class for Member Registration field values
    /// </summary>
    public class MP_Registration : ProjectBasePage
    {       
        /// <summary>
        /// Constructor method to initialize field values
        /// </summary>
        /// <param name="driverContext"></param>
        public MP_Registration(DriverContext driverContext)
     : base(driverContext)
        {
            Common common = new Common(DriverContext);
            FirstName = "WEB_" + common.RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            LastName = "WEB_" + common.RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            EmailAddress = FirstName + "@this.com";
            UserName = FirstName;
            Password = "Password1*";
            ConfirmPassword = Password;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
