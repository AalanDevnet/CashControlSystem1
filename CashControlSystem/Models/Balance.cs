using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashControlSystem.Models
{
    public class Balance
    {
        public int BalanceID { get; set; }
        public int TellerID { get; set; }
        public DateTime Date { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }


    }

}