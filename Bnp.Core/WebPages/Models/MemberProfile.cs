using Bnp.Core.Tests.API.Validators;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Helpers;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using RandomNameGenerator;


namespace Bnp.Core.WebPages.Models
{
    public class MemberProfile : ProjectBasePage
    {

        public MemberProfile(DriverContext driverContext)
      : base(driverContext)
        {
        }
        public Member GenerateMemberBasicInfo()
        {
            Common common = new Common(DriverContext);
            Member member = new Member();
            member.FirstName = "WEB" + RandomDataHelper.RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.LastName = "WEB" + RandomDataHelper.RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.MiddleName = "WEB" + RandomDataHelper.RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);

            member.Username = member.FirstName;
            member.Password = "Password1*";
            member.PrimaryEmailAddress = member.FirstName + "@test.com";
            member.PrimaryPhoneNumber = new System.Random().Next(1, 9999999).ToString();
            return member;
        }

        public MemberDetails GenerateMemberDetails()
        {
            MemberDetails memberdetails = new MemberDetails();
            memberdetails.AddressLineOne = "WEB_Address1_"+ RandomDataHelper.RandomString(3);
            memberdetails.AddressLineTwo = "WEB_Address2_" + RandomDataHelper.RandomString(3);
            memberdetails.City = "austin";
            memberdetails.StateOrProvince = "Texas";
            memberdetails.Country = "USA";
            memberdetails.Gender = "Female";
            memberdetails.ZipOrPostalCode = "765456";
            memberdetails.MobilePhone = new System.Random().Next(1, 9999999).ToString();
            memberdetails.HomePhone = new System.Random().Next(1, 9999999).ToString();
            memberdetails.WorkPhone = new System.Random().Next(1, 9999999).ToString();

            return memberdetails;
        }


        public Member GenerateMemberBasicInfoWithAboveMaxValues()
        {
            Common common = new Common(DriverContext);
            Member member = new Member();
            member.FirstName = "WEB" + RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(51,100)) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.LastName = "WEB" + RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(51, 100)) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.MiddleName = "WEB" + RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(51, 100)) + "_" + NameGenerator.GenerateFirstName(Gender.Male);

            member.Username = "WEB" + RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(255, 300));
            member.Password = "Password1*";
            member.PrimaryEmailAddress = "WEB"+ RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(255, 300)) + "@test.com";
            member.PrimaryPhoneNumber = RandomDataHelper.RandomNumber(RandomDataHelper.RandomNumber(26, 50)).ToString();
           
            return member;
        }

        public MemberDetails GenerateMemberDetailsWithAboveMaxValues()
        {
            MemberDetails memberdetails = new MemberDetails();
            memberdetails.AddressLineOne = "WEB_Address1_" + RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(101, 150));
            memberdetails.AddressLineTwo = "WEB_Address2_"+ RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(101, 150));
            memberdetails.City = "Web_City_"+ RandomDataHelper.RandomString(RandomDataHelper.RandomNumber(51, 100));
            memberdetails.StateOrProvince = "Texas";
            memberdetails.Country = "USA";
            memberdetails.Gender = "Female";
            memberdetails.ZipOrPostalCode = RandomDataHelper.RandomNumber(RandomDataHelper.RandomNumber(26, 50)).ToString();
            memberdetails.MobilePhone= RandomDataHelper.RandomNumber(RandomDataHelper.RandomNumber(26, 50)).ToString();
            memberdetails.HomePhone = RandomDataHelper.RandomNumber(RandomDataHelper.RandomNumber(26, 50)).ToString();
            memberdetails.WorkPhone = RandomDataHelper.RandomNumber(RandomDataHelper.RandomNumber(26, 50)).ToString();

            return memberdetails;
        }
        public VirtualCard GenerateVirtualCard()
        {
            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = new System.Random().Next(1, 9999999).ToString();
            return vc;
        }


        public VirtualCard GenerateVirtualCardAboveMaxValues()
        {
            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = RandomDataHelper.RandomNumber(RandomDataHelper.RandomNumber(255, 300)).ToString();
            return vc;
        }
    }
}
