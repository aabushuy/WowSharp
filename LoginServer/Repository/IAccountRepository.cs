using LoginServer.Entity;

namespace LoginServer.Repository
{
    internal interface IAccountRepository
    {
        Task<Account?> GetAccount(string username);

        Task<Account> RegisterAccount(string username, string password);
    }
}
