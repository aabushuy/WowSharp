namespace RealmSrv.Entity.Requests
{
    internal abstract class Request
    {
        private readonly UserSession _session;

        public UserSession Session => _session;

        public Request(UserSession session)
        {
            _session = session;
        }

        public abstract Task Read();
    }
}
