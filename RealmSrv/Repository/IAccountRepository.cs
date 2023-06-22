using RealmSrv.Entity;

namespace RealmSrv.Repository
{
    internal interface IAccountRepository
    {
        Task<Account?> GetAccount(string userName);

        Task<Account> RegisterAccount(string userName, string password);
    }
}
