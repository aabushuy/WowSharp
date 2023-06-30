namespace RealmSrv.Entity
{
    internal abstract class RequestResponse
    {
        private readonly UserContext _userContext;

        public UserContext UserContext => _userContext;

        public RequestResponse(UserContext context)
        {
            _userContext = context;
        }
    }
}
