using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashControlSystem.Models
{
    public class TransactionSearchViewModel
    {
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public string CustomerCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<TransactionViewModel> Transactions { get; internal set; }
    }
}