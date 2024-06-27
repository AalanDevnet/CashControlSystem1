using CashControlSystem.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.IO;

namespace CashControlSystem.Service
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportToExcel(IEnumerable<TransactionViewModel> transactions)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transactions");

                // Add headers
                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "Customer Code";
                worksheet.Cells[1, 3].Value = "Transaction Type";
                worksheet.Cells[1, 4].Value = "Currency";
                worksheet.Cells[1, 5].Value = "Amount";
                worksheet.Cells[1, 6].Value = "Transaction Date Time";
                worksheet.Cells[1, 7].Value = "Bank PIC";

                // Format headers
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Add data
                var rowIndex = 2;
                foreach (var transaction in transactions)
                {
                    worksheet.Cells[rowIndex, 1].Value = transaction.No;
                    worksheet.Cells[rowIndex, 2].Value = transaction.CustomerCode;
                    worksheet.Cells[rowIndex, 3].Value = transaction.TransactionType;
                    worksheet.Cells[rowIndex, 4].Value = transaction.Currency;
                    worksheet.Cells[rowIndex, 5].Value = transaction.Amount;
                    worksheet.Cells[rowIndex, 6].Value = transaction.TransactionDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[rowIndex, 7].Value = transaction.BankPIC;
                    rowIndex++;
                }

                // Autofit columns
                worksheet.Cells.AutoFitColumns(0);

                // Return as byte array
                return package.GetAsByteArray();
            }
        }
    }
}