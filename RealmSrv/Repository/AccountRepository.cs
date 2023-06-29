using RealmSrv.Entity;
using System.Security.Cryptography;
using System.Text;

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

        
        public Task<Account?> GetAccount(string username)
        {
            var accountInfo = _accounts.FirstOrDefault(a => a.Username == username);

            return Task.FromResult(accountInfo);
        }

        public Task<Account> RegisterAccount(string username, string password)
        {
            username = username.ToUpper();
            password = password.ToUpper();

            var passwordStr = Encoding.ASCII.GetBytes(username + ":" + password);
            var passwordHash = SHA1.HashData(passwordStr);
            var hashStr = BitConverter.ToString(passwordHash).Replace("-", "");

            var acc = new Account()
            { 
                Username = username,
                PasswordHash = hashStr
            };

            return Task.FromResult(acc);
        }
    }
}
