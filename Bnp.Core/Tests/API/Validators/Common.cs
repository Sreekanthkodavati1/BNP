using Bnp.Core.Tests.API.REST_Services.REST_Methods;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Web;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using RandomNameGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using static Bnp.Core.Tests.API.REST_Services.REST_Models.Member_Model;

namespace Bnp.Core.Tests.API.Validators
{
	public class Common : ProjectBasePage
	{

		public Common(DriverContext driverContext)
		   : base(driverContext)
		{
        }

        
        /// <summary>
        /// converts the httpwebresponse object to string
        /// </summary>
        /// <param name="response">web response</param>
        /// <returns>string format of webresponse</returns>
        public String ResponseToString(HttpWebResponse response)
        {
            String responseString = "";
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseString = reader.ReadToEnd();
                    }
            }
            return responseString;
        }

        /// <summary>
        /// It generates pre enrolled member for SOAP service
        /// </summary>
        /// <returns></returns>
        public Member GenerateAddMemberPreEnrolledForSOAP()
        {
            Member member = new Member();
            member.FirstName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.LastName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.FirstName;
            member.Password = "Password1*";
            member.MemberCreateDate = System.DateTime.Now;
            Logger.Info(member.FirstName);
            member.UpdateTransientProperty("initialstatus", "preenrolled");
            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = new System.Random().Next(1, 9999999).ToString();
            member.Add(vc);
            return member;
        }
		/// <summary>
		/// It generates non member for SOAP service
		/// </summary>
		/// <returns></returns>
		public Member GenerateAddNonMemberForSOAP()
		{
			Member member = new Member();
			member.FirstName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
			member.LastName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
			member.Username = member.FirstName;
			member.Password = "Password1*";
			member.MemberCreateDate = System.DateTime.Now;
			Logger.Info(member.FirstName);
			member.UpdateTransientProperty("initialstatus", "nonmember");			
			VirtualCard vc = new VirtualCard();
			vc.LoyaltyIdNumber = new System.Random().Next(1, 9999999).ToString();
			member.Add(vc);
			return member;
		}

		/// <summary>
		/// converts the class object to jsonstring format
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public string ConvertToJsonBody(object obj)
        {
            String jstring;
            try
            {
                jstring = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                return jstring;
            }
            catch
            {
                throw new Exception("unable to convert in  json string");
            }
        }

		/// <summary>
		/// It generates add member using SOAP service
		/// </summary>
		/// <returns>Memeber Details</returns>
        public Member GenerateAddMemberForSOAP()
        {
            Member member = new Member();
            member.FirstName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.LastName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.FirstName;
            member.Password = "Password1*";
            member.MemberCreateDate = System.DateTime.Now;
            Logger.Info(member.FirstName);
            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = new System.Random().Next(1, 9999999).ToString();
            member.Add(vc);
            return member;
        }

		/// <summary>
		/// It generates add member with mandatory fields using SOAP service
		/// </summary>
		/// <returns>Member Details</returns>
        public Member GenerateaddMemberForSOAPMandatory()
        {
            Member member = new Member();
            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = new System.Random().Next(1, 9999999).ToString();
            member.Add(vc);
            return member;
        }

		/// <summary>
		/// It generates add member with no VCKey using SOAP service
		/// </summary>
		/// <returns>Member Details</returns>
        public Member GenerateAddMemberForSOAPNoVC()
        {
            Member member = new Member();
            member.FirstName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.LastName = "CDIS_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.FirstName;
            member.Password = "Password1*";
            member.MemberCreateDate = System.DateTime.Now;
            Logger.Info(member.FirstName);
            return member;
        }

		/// <summary>
		/// It generates add member with existing VCKey using SOAP service
		/// </summary>
		/// <returns>Member Details</returns>
        public Member GenerateAddMemberForSOAPExistingVC()
        {
            Member member = new Member();
            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = DatabaseUtility.GetExistingLoyaltyCardIDwithActiveStatus();
            member.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member with all fields using SOAP service
        /// </summary>
        /// <returns>Member Details</returns>
        public Member AddMemberWithAllFields()
        {
            Member member = new Member();
            member.IpCode = Convert.ToInt64(RandomNumber(6));
            member.MemberCreateDate = System.DateTime.Now;
            member.MemberCloseDate = System.DateTime.Now.AddYears(20);
            member.BirthDate = System.DateTime.Now.AddYears(-20);
            member.FirstName = "SOAP_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.LastName = "SOAP_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.MiddleName = "SOAP_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.NamePrefix = "MR.";
            member.NameSuffix = "";
            member.Username = member.FirstName;
            member.Password = "Password1*";
            member.PrimaryEmailAddress = member.FirstName + "@Test.Com";
            member.PrimaryPhoneNumber = "7" + RandomNumber(9);
            member.PrimaryPostalCode = RandomNumber(1) + RandomNumber(4);
            member.PreferredLanguage = "en";
            member.IsEmployee = false;
            member.ChangedBy = "SOAP_Automation";

            MemberDetails details = new MemberDetails();
            details.EmailOptIn = true;
            details.EmailOptInDate = System.DateTime.Now;
            details.AddressLineOne = "Automation Test Dr";
            details.AddressLineTwo = "APT" + RandomNumber(4);
            details.AddressLineThree = "";
            details.AddressLineFour = "";
            details.City = "Plano";
            details.County = "Collin";
            details.StateOrProvince = "Texas";
            details.Country = "USA";
            details.SecurityQuestion = "Which city you were born";
            details.SecurityAnswer = "Dallas";
            details.TAndCAgreed = true;
            details.ZipOrPostalCode = member.PrimaryPostalCode;
            details.Gender = Gender.Male.ToString();
            details.SmsOptIn = true;
            details.MobilePhoneCountryCode = 1;
            details.MobilePhone = member.PrimaryPhoneNumber;

            details.HomePhone = null;
            details.HomeStoreID = null;
            details.WorkPhone = null;
            details.SecondaryEmailAddress = null;
            details.EmailAddressMailable = null;
            details.MemberStatusCode = null;
            details.AddressMailable = null;
            details.DirectMailOptIn = null;
            details.DirectMailOptInDate = null;
            details.SmsDblOptInComplete = null;
            details.FacebookOptIn = null;
            details.TwitterOptIn = null;
            details.EnrollDate = null;
            details.MemberSource = null;
            details.SmsConsentChangeDate = null;
            details.FacebookOptInDate = null;
            details.TwitterOptInDate = null;

            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = new System.Random().Next(1, 9999999).ToString();
            vc.IpCode = member.IpCode;
            vc.VcKey = Convert.ToInt64(RandomNumber(8));
            vc.DateIssued = System.DateTime.Now;
            vc.DateRegistered = System.DateTime.Now;
            vc.IsPrimary = true;
            member.AlternateId = vc.LoyaltyIdNumber;
            member.Add(vc);
            member.Add(details);
            return member;
        }
         
        /// <summary>
        /// It generates add member using REST service
        /// </summary>
        /// <returns>Member Details</returns>
        public Memberm GenerateAddMemberForREST()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service where expiration date is less than system date
        /// </summary>
        /// <returns>Date</returns>
        public Memberm GenerateAddMemberForRESTWhereExpirationDateLessThanSysDate()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            vc.expirationDate = string.Format("{0:s}", DateTime.UtcNow.AddDays(-7));
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service where expiration date is greater than system date
        /// </summary>
        /// <returns>Date</returns>
        public Memberm GenerateAddMemberForRESTWhereExpirationDateGreaterThanSysDate()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            vc.expirationDate = string.Format("{0:s}", DateTime.UtcNow.AddDays(7));
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service where expiration date is equal to system date
        /// </summary>
        /// </summary>
        /// <returns>Date</returns>
        public Memberm GenerateAddMemberForRESTWhereExpirationDateEqualsToSysDate()
        {
            Memberm member = new Memberm();
            member.firstName = "REST1_" + RandomString(9) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST1_" + RandomString(9) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 99999999).ToString();
            vc.dateIssued= string.Format("{0:s}", DateTime.Now.AddDays(-10));
            vc.dateRegistered = vc.dateIssued;
            vc.expirationDate = string.Format("{0:s}", DateTime.Now.AddSeconds(10));
            member.cards.Add(vc);
            return member;
        }
        /// <summary>
        ///  It generates add member using REST service with invalid Zip Code
        /// </summary>
        /// <returns>ZipCode</returns>
        public Memberm GenerateAddMemberForRESTWithInvalidZipCode()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.zipCode = "ZIP_" + RandomNumber(20);
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }
        /// <summary>
        /// It generates add member using REST service with invalid Phone number
        /// </summary>
        /// <returns>Phone Number</returns>
        public Memberm GenerateAddMemberForRESTWithInvalidPhoneNumber()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.phone = RandomNumber(15);
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service with Alternate Id in quotes
        /// </summary>
        /// <returns>Alternate Id</returns>
        public Memberm GenerateAddMemberForRESTWhereAlternateIdInQuotes()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.AlternateId = "\"4521";
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service with same username and passsword
        /// </summary>
        /// <returns>Username and Password</returns>
        public Memberm GenerateAddMemberForRESTWithSameUsernameAndPassword()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = member.firstName;
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service with password as number
        /// </summary>
        /// <returns>Password</returns>
        public Memberm GenerateAddMemberForRESTByProvidingPasswordAsNumber()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = RandomNumber(10);
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service with password less than 7 characters
        /// </summary>
        /// <returns>password</returns>
        public Memberm GenerateAddMemberForRESTByProvidingPasswordLessThan7Chars()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = RandomString(3);
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service by providing password only
        /// </summary>
        /// <returns>password</returns>
        public Memberm GenerateAddMemberForRESTByProvidingPasswordOnly()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service by providing username only
        /// </summary>
        /// <returns>username</returns>
        public Memberm GenerateAddMemberForRESTByProvidingUsernameOnly()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service by providing invalid birthdate
        /// </summary>
        /// <returns>birthdate</returns>
        public Memberm GenerateAddMemberForRESTWithInvalidBirthDate()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.birthDate = "REST_" + RandomString(8);
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service by providing invalid Is Employee
        /// </summary>
        /// <returns>IsEmployee</returns>
        public InValidMember GenerateAddMemberForRESTWithInvalidEmployee()
        {
            InValidMember inValidMember = new InValidMember();
            inValidMember.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            inValidMember.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            inValidMember.Username = inValidMember.firstName;
            inValidMember.Password = "Password1*";
            inValidMember.email = inValidMember.firstName + "@test.com";
            inValidMember.cards = new List<card>();
            inValidMember.isEmployee = "EMP" + RandomString(6);
            Logger.Info(inValidMember.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            inValidMember.cards.Add(vc);
            return inValidMember;
        }

        /// <summary>
        /// It generates add member using REST service by providing values exceeding maximum characters
        /// </summary>
        /// <returns>member objects</returns>
        public Memberm GenerateAddMemberForRESTWithValuesExceedingMaximumCharacters()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(60) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(60) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.middleName = "REST_" + RandomString(60) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = RandomString(60);
            member.Password = RandomString(60);
            member.email = RandomString(260) + "@test.com";
            member.phone = RandomNumber(30);
            member.zipCode = RandomNumber(20);
            member.namePrefix = RandomString(20);
            member.nameSuffix = RandomString(20);
            member.AlternateId = RandomString(260);
            member.changedBy = RandomString(30);
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = RandomNumber(260);
            member.cards.Add(vc);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service by providing multiple cards
        /// </summary>
        /// <returns>Cards</returns>
        public Memberm GenerateAddMemberMultipleCardsForREST()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            card vc1 = new card();
            vc1.cardNumber = new System.Random().Next(1, 9999999).ToString();
            member.cards.Add(vc);
            member.cards.Add(vc1);
            return member;
        }

        /// <summary>
        /// It generates add member using REST service by providing invalid multiple attribute sets
        /// </summary>
        /// <returns></returns>
        public Memberm GenerateAddMemberMultipleAttributeSetForREST()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.email = member.firstName + "@test.com";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            member.attributeSets = new List<AttributeSets>();
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            AttributeSets details = new AttributeSets();
            details.memberDetails = new REST_Services.REST_Models.AttributeSet_Model();
              details.memberDetails = REST_DataGenerator.GenerateMemberAttributeSet(this);
            vc.cardNumber = new System.Random().Next(1, 9999999).ToString();
            //AttributeSets details1 = new AttributeSets();
            //details1.memberDetails = new REST_Services.REST_Models.AttributeSet_Model();
            //details1.memberDetails = REST_DataGenerator.GenerateMemberAttributeSet(this);
            member.cards.Add(vc);
            member.attributeSets.Add(details);

           //member.attributeSets.Add(details1);          
            return member;
        }

        /// <summary>
        ///  It generates add member with mandatory fields using REST service
        /// </summary>
        /// <returns>Member Details</returns>
        public Member GenerateAddMemberForRESTMandatory()
        {
            Member member = new Member();
            member.FirstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.LastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.FirstName;
            member.Password = "Password1*";
            return member;
        }

		/// <summary>
		///  It generates add member with existing VCKey using REST service
		/// </summary>
		/// <returns>Member Details</returns>
		public Memberm GenerateAddMemberForRESTExistingVC()
        {
            Memberm member = new Memberm();
            member.firstName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.lastName = "REST_" + RandomString(6) + "_" + NameGenerator.GenerateFirstName(Gender.Male);
            member.Username = member.firstName;
            member.Password = "Password1*";
            member.cards = new List<card>();
            Logger.Info(member.firstName);
            card vc = new card();
            vc.cardNumber = DatabaseUtility.GetExistingLoyaltyCardIDwithActiveStatusREST();
            member.cards.Add(vc);
            return member;
        }

		/// <summary>
		/// It generates random number
		/// </summary>
		/// <param name="length">Specifies the length of the number</param>
		/// <returns>Random Number</returns>
		public string RandomNumber(int length)
        {
            const string chars = "0123456789";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

		/// <summary>
		/// It generates random string
		/// </summary>
		/// <param name="length">Specifies the length of the string</param>
		/// <returns>Random String</returns>
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }




       
    }
}
