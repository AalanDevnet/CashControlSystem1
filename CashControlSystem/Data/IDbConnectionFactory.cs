using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CashControlSystem.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}