namespace RealmSrv.Entity.Responses
{
    internal abstract class Response
    {
        protected readonly UserSession _userContext;

        public Response(UserSession userContext)
        {
            _userContext = userContext;
        }

        public abstract Task Write();
    }
}
