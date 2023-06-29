using RealmSrv.Entity;

namespace RealmSrv.Repository
{
    internal interface IRealmRepository
    {
        Task<IList<Realm>> GetRealmListForAccount(Account account);
    }
}
