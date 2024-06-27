using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CashControlSystem.Models;
using Dapper;

namespace CashControlSystem.IRepo
{
    public interface ITransactionRepository
    {
        bool DepositTransaction(Transaction transaction);
        IEnumerable<TransactionViewModel> SearchDailyTransactions(DateTime startDate, DateTime endDate, string transactionType, string currency, string customerCode);
        IEnumerable<TransactionViewModel> GenerateReport(DateTime startDate, DateTime endDate);
        List<TransactionViewModel> GetTransactions();
    }

}