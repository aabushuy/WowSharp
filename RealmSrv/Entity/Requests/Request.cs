namespace RealmSrv.Entity.Requests
{
    internal abstract class Request : RequestResponse
    {
        protected Request(UserSession session) : base(session)
        {
        }

        public abstract Task Read();
    }
}
