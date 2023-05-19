using Domain.Entity.Account;

namespace DataAccess.Repositories.Instances
{
    internal class AccountInfoRepository : IAccountInfoRepository
    {
        public Task AddAccount(AccountInfo accountInfo)
        {
            throw new NotImplementedException();
        }

        //TODO:
        public Task<AccountInfo> GetAccountInfo(string login)
        {
            var accountInfo = new AccountInfo()
            {
                Username = "qwer",
                PasswordHash = "BCC8A524726544C45375256397848DFDF4E19C1B"              
            };

            return Task.FromResult(accountInfo);
        }
    }
}
