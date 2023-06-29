using RealmSrv.Entity;

namespace RealmSrv.Repository
{
    internal class RealmRepository : IRealmRepository
    {
        public Task<IList<Realm>> GetRealmListForAccount(Account account)
        {
            var result = new List<Realm>() 
            { 
                new Realm("Common", "localhost", 4545)
            };

            return Task.FromResult(result as IList<Realm>);
        }
    }
}
