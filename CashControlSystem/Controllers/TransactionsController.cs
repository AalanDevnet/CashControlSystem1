using CashControlSystem.IRepo;
using CashControlSystem.Models;
using CashControlSystem.Service;
using System;
using System.Web.Mvc;
using System.Linq;
using OfficeOpenXml;
using System.IO;

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

        // GET: Transactions/Deposit
        public ActionResult Deposit()
        {
            int userId = (Session["User"] as User)?.UserId ?? 0;

            Customer customer = _transactionRepository.GetCustomerByUserId(userId);

            var transactionViewModel = new TransactionViewModel
            {
                CustomerCode = customer?.CustomerCode,
            };

            return View(transactionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit(TransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = (Session["User"] as User)?.UserId ?? 0;
                Customer customer = _transactionRepository.GetCustomerByUserId(userId);

                if (customer == null)
                {
                    return Json(new { success = false, message = "Customer not found." });
                }

                var transaction = new Transaction
                {
                    CustomerID = customer.CustomerID,
                    TransactionType = "Deposit",
                    Currency = model.Currency,
                    Amount = model.Amount,
                    TransactionDateTime = DateTime.Now,
                    BankPIC = model.BankPIC
                };

                bool depositSuccess = _transactionRepository.DepositTransaction(transaction);

                if (depositSuccess)
                {
                    bool addToBalanceSuccess = _transactionRepository.AddToBalance(customer.CustomerID, model.Currency, model.Amount);

                    if (addToBalanceSuccess)
                    {
                        return Json(new { success = true, message = "Deposit and balance update successful." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to update balance." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Failed to deposit transaction." });
                }
            }

            return Json(new { success = false, message = "Invalid input." });
        }

        public ActionResult Report()
        {
            int userId = (Session["User"] as User)?.UserId ?? 0;

            Customer customer = _transactionRepository.GetCustomerByUserId(userId);

            var transactions = _transactionRepository.SearchDailyTransactions(
                null,              
                null,              
                null,              
                null,             
                null               
            );

            return View(transactions);
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

        public ActionResult ExportToExcel()
        {
            int userId = (Session["User"] as User)?.UserId ?? 0;
            var customer = _transactionRepository.GetCustomerByUserId(userId);

            var transactions = _transactionRepository.SearchDailyTransactions(
                null,  
                null,  
                null,  
                null, 
                customer?.CustomerCode  
            );

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transactions");
                worksheet.Cells["A1"].LoadFromCollection(transactions, true);

                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    string fileName = $"Transactions_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return File(stream.ToArray(), contentType, fileName);
                }
            }
        }
    }
}
