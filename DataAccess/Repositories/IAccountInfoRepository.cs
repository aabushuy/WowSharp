using Domain.Entity.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IAccountInfoRepository
    {
        Task<AccountInfo> GetAccountInfo(string login);

        Task AddAccount(AccountInfo accountInfo);
    }
}
