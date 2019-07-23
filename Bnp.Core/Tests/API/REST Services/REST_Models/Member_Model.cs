using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bnp.Core.Tests.API.REST_Services.REST_Models
{
    public class Member_Model
    {
        public class Memberm
        {
            public string birthDate { get; set; }
            public string firstName { get; set; }
            public bool isEmployee { get; set; }
            public string lastName { get; set; }
            public string memberStatus { get; set; }
            public string middleName { get; set; }
            public string namePrefix { get; set; }
            public string nameSuffix { get; set; }
            public string Password { get; set; }
            public string preferredLanguage { get; set; }
            public string passwordExpireDate { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string zipCode { get; set; }
            public string Username { get; set; }
            public string AlternateId { get; set; }
            public string changedBy { get; set; }
            public int[] storeLocations { get; set; }
            public List<card> cards { get; set; }
            public List<AttributeSets> attributeSets { get; set; }
        }
        public class card
        {
            public string cardNumber { get; set; }
            public string dateIssued { get; set; }
            public string dateRegistered { get; set; }
            public string expirationDate { get; set; }
            public bool isPrimary { get; set; }
            public int CardType { get; set; }
        }


        public class AttributeSets
        {
            public AttributeSet_Model memberDetails { get; set; }
        }

        public class InValidMember
        {
            public string birthDate { get; set; }
            public string firstName { get; set; }
            public string isEmployee { get; set; }
            public string lastName { get; set; }
            public string memberStatus { get; set; }
            public string middleName { get; set; }
            public string namePrefix { get; set; }
            public string nameSuffix { get; set; }
            public string Password { get; set; }
            public string preferredLanguage { get; set; }
            public string passwordExpireDate { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string zipCode { get; set; }
            public string Username { get; set; }
            public string AlternateId { get; set; }
            public string changedBy { get; set; }
            public int[] storeLocations { get; set; }
            public List<card> cards { get; set; }
            public List<AttributeSets> attributeSets { get; set; }
        }
    }

}



