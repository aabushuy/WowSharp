using LoginServer.Entity;

namespace LoginServer.Repository
{
    internal interface IRealmRepository
    {
        Task<IList<Realm>> GetRealmListForAccount(Account account);
    }
}
