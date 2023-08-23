using LoginServer.Entity;

namespace LoginServer.Repository
{
    internal class RealmRepository : IRealmRepository
    {
        public Task<IList<Realm>> GetRealmListForAccount(Account account)
        {
            var result = new List<Realm>() 
            {
                new Realm("Spring", "127.0.0.1", 4545)
                {
                   RealmFlags = RealmFlag.ForceNewPlayers
                }
            };

            return Task.FromResult(result as IList<Realm>);
        }
    }
}
