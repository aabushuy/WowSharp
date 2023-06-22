namespace RealmSrv.Entity.Requests
{
    internal abstract class Request
    {
        private readonly UserContext _userContext;

        public UserContext User => _userContext;

        public Request(UserContext userContext)
        {
            _userContext = userContext;
        }

        public abstract Task Read();
    }
}
