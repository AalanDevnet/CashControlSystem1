﻿using CashControlSystem.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using CashControlSystem.Data;
using CashControlSystem.Models;

namespace CashControlSystem.Repo
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public TransactionRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public bool DepositTransaction(Transaction transaction)
        {
            using (var db = _dbConnectionFactory.CreateConnection())
            {
                string sql = "EXEC usp_DepositTransaction @CustomerID, @TransactionType, @Currency, @Amount, @TransactionDateTime, @BankPIC";
                var result = db.Execute(sql, new
                {
                    transaction.CustomerID,
                    transaction.TransactionType,
                    transaction.Currency,
                    transaction.Amount,
                    transaction.TransactionDateTime,
                    transaction.BankPIC
                });
                return result > 0;
            }
        }

        public IEnumerable<TransactionViewModel> SearchDailyTransactions(DateTime startDate, DateTime endDate, string transactionType, string currency, string customerCode)
        {
            using (var db = _dbConnectionFactory.CreateConnection())
            {
                string sql = "EXEC usp_SearchDailyTransactions @StartDate, @EndDate, @TransactionType, @Currency, @CustomerCode";
                return db.Query<TransactionViewModel>(sql, new { startDate, endDate, transactionType, currency, customerCode });
            }
        }

        public IEnumerable<TransactionViewModel> GenerateReport(DateTime startDate, DateTime endDate)
        {
            using (var db = _dbConnectionFactory.CreateConnection())
            {
                string sql = "EXEC usp_GenerateReport @StartDate, @EndDate";
                return db.Query<TransactionViewModel>(sql, new { startDate, endDate });
            }
        }

        public List<TransactionViewModel> GetTransactions()
        {
            using (var db = _dbConnectionFactory.CreateConnection())
            {
                string sql = "EXEC usp_GetTransactionsWithCustomerInfo";
                var transactions = db.Query<TransactionViewModel>(sql).ToList();
                return transactions;
            }
        }
    }



}