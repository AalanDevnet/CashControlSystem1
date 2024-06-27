using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashControlSystem.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int CustomerID { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string BankPIC { get; set; }
    }

   

}