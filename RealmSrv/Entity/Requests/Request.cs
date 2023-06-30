namespace RealmSrv.Entity.Requests
{
    internal abstract class Request : RequestResponse
    {
        protected Request(UserContext userContext) : base(userContext)
        {
        }

        public abstract Task Read();
    }
}
