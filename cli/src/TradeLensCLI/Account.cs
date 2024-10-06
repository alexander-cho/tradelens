using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeLensCLI
{
    public class Account
    {
        // public DateTime CreatedAt;
        // static Account(DateTime createdAt)
        // {
        //     CreatedAt = createdAt;
        // }

        private string _accountType;

        public void SetAccountType(string accountType)
        {
            _accountType = accountType;
        }

        public string GetAccountType()
        {
            Console.WriteLine($"You are accessing your {_accountType}");
            return _accountType;
        }
    }
}
