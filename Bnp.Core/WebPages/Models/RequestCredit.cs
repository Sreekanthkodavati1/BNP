using Bnp.Core.Tests.API.Validators;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.Models
{
   public class RequestCredit : ProjectTestBase
    {
    
        public string TransactionNumber { get; set; }
        public string StoreNumber{ get; set; }
        public string TxnDate{ get; set; }
        public string TxnAmount { get; set; }
        public string RegisterNumber { get; set; }
        public string OrderNumber { get; set; }


    }
}
