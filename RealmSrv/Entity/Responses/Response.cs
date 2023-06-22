namespace RealmSrv.Entity.Responses
{
    internal abstract class Response
    {
        protected readonly UserContext _userContext;

        public Response(UserContext userContext)
        {
            _userContext = userContext;
        }

        public abstract Task Write();
    }
}
