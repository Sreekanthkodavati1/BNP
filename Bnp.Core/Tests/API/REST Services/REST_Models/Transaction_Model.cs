using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.REST_Services.REST_Models
{
	public class Transaction_Model
	{
		public class Transaction
		{
			public string transactionId { get; set; }
			public string brandId { get; set; }
			public int transactionType { get; set; }
			public int creditsUsed { get; set; }
			public int creditCardId { get; set; }
			public string maskId { get; set; }
			public string transactionNumber { get; set; }
			public DateTime transactionDate { get; set; }
			public string registerNumber { get; set; }
			public int storeId { get; set; }
			public string webOrderNumber { get; set; }
			public double amount { get; set; }
			public double qualifyingAmount { get; set; }
			public double discountAmount { get; set; }
			public string employeeId { get; set; }
			public string channel { get; set; }
			public List<Items> items { get; set; }
			public List<Discounts> discounts { get; set; }
			public List<Tenders> tenders { get; set; }
			public List<Rewards> rewards { get; set; }
		}
		public class Items
		{
			public double discountAmount { get; set; }
			public string itemId { get; set; }
			public int productId { get; set; }
			public int itemTypeId { get; set; }
			public int clearanceItem { get; set; }
			public int itemNumber { get; set; }
			public double retailAmount { get; set; }
			public double saleAmount { get; set; }
			public int actionId { get; set; }
			public int quantity { get; set; }
            public string transactionId { get; set; }
		}
		public class Discounts
		{
			public double discountAmount { get; set; }
			public string offerCode { get; set; }
			public string discountId { get; set; }
			public string itemId { get; set; }
			public string discountType { get; set; }
			public string transactionId { get; set; }
			public DateTime transactionDate { get; set; }
		}
		public class Tenders
		{
			public string tenderId { get; set; }
			public int tenderType { get; set; }
			public double amount { get; set; }
			public string currency { get; set; }
			public double taxRate { get; set; }
			public double taxAmount { get; set; }
			public string transactionId { get; set; }
		}
		public class Rewards
		{
			public string itemId { get; set; }
			public int rewardType { get; set; }
			public string rewardCode { get; set; }
			public double discountAmount { get; set; }
			public int programId { get; set; }
			public string transactionId { get; set; }
			public DateTime transactionDate { get; set; }
		}

	}
}


