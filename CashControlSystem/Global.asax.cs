using CashControlSystem.Data;
using CashControlSystem.IRepo;
using CashControlSystem.Repo;
using CashControlSystem.Service;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

namespace CashControlSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           // ExcelPackage.LicenseContext = System.ComponentModel.LicenseContext.NonCommercial;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer();
            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
        private void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IDbConnectionFactory, DbConnectionFactory>();
            container.RegisterType<ITransactionRepository, TransactionRepository>();
            container.RegisterType<IExcelExportService, ExcelExportService>();
            container.RegisterType<IUserRepository, UserRepository>();
        }

    }
}
