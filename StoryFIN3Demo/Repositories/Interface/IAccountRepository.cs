using DemoFIN3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFIN3.Core.Repositories.Interface
{
    public interface IAccountRepository : IDisposable
    {
        Account Find(int accountId);
        void AddAccount(Account account);
        void UpdateAccount(Account account);
        void DeleteAccount(Account account);
        void DeleteAccount(int accountId);
        IList<Account> GetAllAccounts();
    }
}
