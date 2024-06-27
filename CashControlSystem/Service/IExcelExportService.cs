using CashControlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashControlSystem.Service
{
    public interface IExcelExportService
    {
        byte[] ExportToExcel(IEnumerable<TransactionViewModel> transactions);
    }
}