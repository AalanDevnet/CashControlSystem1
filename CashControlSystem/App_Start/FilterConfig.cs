using System.Web;
using System.Web.Mvc;
using CashControlSystem.Helpers;

namespace CashControlSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionCheckAttribute());
        }
    }
}
