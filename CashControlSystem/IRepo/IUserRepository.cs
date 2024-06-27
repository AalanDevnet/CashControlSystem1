using CashControlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashControlSystem.IRepo
{
    public interface IUserRepository
    {
        User Login(string username, string passwordHash);
    }
}