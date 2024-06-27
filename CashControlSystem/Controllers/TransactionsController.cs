using CashControlSystem.IRepo;
using CashControlSystem.Models;
using CashControlSystem.Service;
using System;
using System.Web.Mvc;
using System.Linq;

namespace CashControlSystem.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IExcelExportService _excelExportService;

        public TransactionsController(ITransactionRepository transactionRepository, IExcelExportService excelExportService)
        {
            _transactionRepository = transactionRepository;
            _excelExportService = excelExportService;
        }

        public ActionResult Index()
        {
            var transactions = _transactionRepository.GetTransactions(); 
            var transactionViewModels = transactions.Select(t => new TransactionViewModel
            {
                No = t.No,
                CustomerCode = t.CustomerCode,
                CustomerName = t.CustomerName,
                TransactionType = t.TransactionType,
                Currency = t.Currency,
                Amount = t.Amount,
                TransactionDateTime = t.TransactionDateTime,
                BankPIC = t.BankPIC
            }).ToList();

            return View(transactionViewModels);
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = _transactionRepository.DepositTransaction(transaction);
                    if (success)
                    {
                        return Json(new { success = true, message = "Deposit transaction successful." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to save deposit transaction." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid input data. Please check your input." });
            }
        }

        [HttpGet]
        public ActionResult SearchDailyTransactions(DateTime startDate, DateTime endDate, string transactionType, string currency, string customerCode)
        {
            try
            {
                var transactions = _transactionRepository.SearchDailyTransactions(startDate, endDate, transactionType, currency, customerCode);
                return Json(transactions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Failed to retrieve transactions: {ex.Message}" });
            }
        }

        [HttpGet]
        public ActionResult ExportToExcel(DateTime startDate, DateTime endDate, string transactionType, string currency, string customerCode)
        {
            try
            {
                var transactions = _transactionRepository.SearchDailyTransactions(startDate, endDate, transactionType, currency, customerCode);
                var fileContents = _excelExportService.ExportToExcel(transactions);

                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transactions.xlsx");
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                return Json(new { success = false, message = $"Failed to export transactions to Excel: {ex.Message}" });
            }
        }
    }
}
