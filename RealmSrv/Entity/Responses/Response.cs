namespace RealmSrv.Entity.Responses
{
    internal abstract class Response : RequestResponse
    {
        protected Response(UserContext userContext) : base(userContext)
        {
        }

        public abstract Task Write();
    }
}
