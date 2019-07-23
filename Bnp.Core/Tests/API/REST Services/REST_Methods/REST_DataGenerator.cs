using Bnp.Core.Tests.API.REST_Services.REST_Models;
using Bnp.Core.Tests.API.Validators;
using System;
using System.Collections.Generic;
using static Bnp.Core.Tests.API.REST_Services.REST_Models.Member_Model;
using static Bnp.Core.Tests.API.REST_Services.REST_Models.Transaction_Model;

namespace Bnp.Core.Tests.API.REST_Services.REST_Methods
{
	public class REST_DataGenerator

	{
		private Common test;
        static System.Random random = new System.Random();

        public REST_DataGenerator(Common test)
		{
			this.test = test;
		}

		public static Transaction GenerateTransactionWithRequiredFields(Common common)
		{
			Transaction transaction = new Transaction();

			transaction.transactionId = common.RandomNumber(12);
			transaction.transactionType = Convert.ToInt32(common.RandomNumber(1));
			transaction.transactionDate = System.DateTime.Now;
			transaction.registerNumber = "1";
			transaction.storeId = 1;
            transaction.amount = Math.Round(random.NextDouble() * 1000, 4);
            transaction.items = new List<Items>();
			transaction.discounts = new List<Discounts>();
			transaction.rewards = new List<Rewards>();
			transaction.tenders = new List<Tenders>();

			return transaction;
		}
		public static Transaction GenerateTransactionWithAllFields(Common common)
		{
			
			Transaction transaction = new Transaction();
			Items item = new Items();
			Discounts discount = new Discounts();
			Rewards reward = new Rewards();
			Tenders tender = new Tenders();

			transaction.transactionId = common.RandomNumber(12);
			transaction.transactionType = Convert.ToInt32(common.RandomNumber(1));
			transaction.transactionDate = System.DateTime.Now;
			transaction.registerNumber = "1";
			transaction.storeId = 1;
			transaction.brandId = common.RandomString(6);
			transaction.creditsUsed = Convert.ToInt32(common.RandomNumber(3));
			transaction.creditCardId = Convert.ToInt32(common.RandomNumber(8));
			transaction.maskId = "xxxxxxxxxxxx" + common.RandomNumber(4);
			transaction.transactionNumber = common.RandomString(8);
			transaction.webOrderNumber = common.RandomString(8);
			transaction.amount = Math.Round(random.NextDouble() * 1000, 4);
			transaction.qualifyingAmount = random.NextDouble() * 100;
			transaction.discountAmount = random.NextDouble() * 10;
			transaction.employeeId = common.RandomString(10);
			transaction.channel = common.RandomString(3);
			transaction.items = new List<Items>();
			transaction.discounts = new List<Discounts>();
			transaction.rewards = new List<Rewards>();
			transaction.tenders = new List<Tenders>();

			item.discountAmount = random.NextDouble() * 10;
			item.itemId = common.RandomString(5);
			item.productId = Convert.ToInt32(common.RandomNumber(3));
			item.itemTypeId = Convert.ToInt32(common.RandomNumber(1));
			item.clearanceItem = Convert.ToInt32(common.RandomNumber(3));
			item.itemNumber = Convert.ToInt32(common.RandomNumber(3));
			item.retailAmount = random.NextDouble() * 100;
			item.saleAmount = random.NextDouble() * 100;
			item.actionId = Convert.ToInt32(common.RandomNumber(1));
            item.transactionId = transaction.transactionId;
            item.quantity = Convert.ToInt32(common.RandomNumber(3));

            discount.discountAmount = random.NextDouble() * 10;
			discount.offerCode = common.RandomString(10);
			discount.discountId = common.RandomString(10);
			discount.itemId = common.RandomString(10);
			discount.discountType = common.RandomString(10);
			discount.transactionId = transaction.transactionId;
			discount.transactionDate = transaction.transactionDate;

			tender.tenderId = common.RandomString(8);
			tender.tenderType = Convert.ToInt32(common.RandomNumber(1));
			tender.amount = random.NextDouble() * 1000;
			tender.currency = common.RandomString(3);
			tender.taxRate = random.NextDouble() * 10;
			tender.taxAmount = random.NextDouble() * 100;
			tender.transactionId = transaction.transactionId;

			reward.itemId = common.RandomString(10);
			reward.rewardType = Convert.ToInt32(common.RandomNumber(1));
			reward.rewardCode = common.RandomString(10);
			reward.discountAmount = random.NextDouble() * 100;
			reward.programId = Convert.ToInt32(common.RandomNumber(1));
			reward.transactionId = transaction.transactionId;
			reward.transactionDate = transaction.transactionDate;

			transaction.items.Add(item);
			transaction.discounts.Add(discount);
			transaction.tenders.Add(tender);
			transaction.rewards.Add(reward);
			return transaction;

		}
		public static AttributeSet_Model GenerateMemberAttributeSet(Common common)
		{
			AttributeSet_Model attribute = new AttributeSet_Model();
			attribute.addressLineOne = common.RandomString(15);
			attribute.city = "Dallas";
			attribute.stateOrProvince = common.RandomNumber(3);
			attribute.zipOrPostalCode = common.RandomNumber(5);
			attribute.country = "USA";

			return attribute;
		}

        public static Memberm GenerateUpdateMemberForREST(Common common)
        {
            Memberm member = new Memberm();
            member.birthDate = DateTime.UtcNow.AddYears(-30).ToLongTimeString();
            member.email = "updateREST_"+common.RandomString(10)+"@test.com";
          
            
            member.middleName = "RESTUpdateMN" + common.RandomString(10);
            member.changedBy = "RESTPatchUpdate";
            member.Password = "Password1*";
            member.isEmployee = false;
            member.passwordExpireDate = DateTime.UtcNow.AddYears(30).ToLongTimeString();
            member.zipCode = "96586";
            member.storeLocations = new int[] { 1, 2, 3, 45 };

            member.attributeSets = new List<AttributeSets>();
                        AttributeSets attribute = new AttributeSets();
            attribute.memberDetails= GenerateMemberAttributeSet(common);
            member.attributeSets.Add(attribute);
            return member;
        }
    }
}
