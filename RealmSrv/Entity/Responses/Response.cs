namespace RealmSrv.Entity.Responses
{
    internal abstract class Response : RequestResponse
    {
        protected Response(UserSession session) : base(session)
        {
        }

        public abstract Task Write();
    }
}
