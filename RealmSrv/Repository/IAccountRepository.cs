using RealmSrv.Entity;

namespace RealmSrv.Repository
{
    internal interface IAccountRepository
    {
        Task<Account?> GetAccount(string username);

        Task<Account> RegisterAccount(string username, string password);
    }
}
