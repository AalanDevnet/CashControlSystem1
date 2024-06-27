using CashControlSystem.Data;
using CashControlSystem.IRepo;
using CashControlSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace CashControlSystem.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public User Login(string username, string passwordHash)
        {
            using (var db = _dbConnectionFactory.CreateConnection())
            {
                string sql = "EXEC usp_Login @Username, @PasswordHash";
                return db.Query<User>(sql, new { Username = username, PasswordHash = passwordHash }).FirstOrDefault();
            }
        }
    }
}