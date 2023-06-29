namespace RealmSrv.Entity
{
    internal abstract class RequestResponse
    {
        private readonly UserSession _session;

        public UserSession Session => _session;

        public RequestResponse(UserSession session)
        {
            _session = session;
        }
    }
}
