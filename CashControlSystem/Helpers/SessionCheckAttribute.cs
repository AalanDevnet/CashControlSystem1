using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CashControlSystem.Helpers
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["User"] == null)
            {
                if (filterContext.ActionDescriptor.ActionName != "Login")
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}