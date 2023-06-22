using RealmSrv.Entity;

namespace RealmSrv.Repository
{
    internal class AccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = new();
        public AccountRepository()
        {
            //TODO: delete
            var test = RegisterAccount("qwer", "123").Result;
            _accounts.Add(test);           
        }

        
        public Task<Account?> GetAccount(string userName)
        {
            var accountInfo = _accounts.FirstOrDefault(a => a.Username.ToUpper() == userName);

            return Task.FromResult(accountInfo);
        }

        public Task<Account> RegisterAccount(string userName, string password)
        {
            var acc = new Account()
            { 
                Username = userName
            };

            return Task.FromResult(acc);
        }
    }
}
