using LoginServer.Entity;

namespace LoginServer.Core.Requests
{
    internal abstract class Request : RequestResponse
    {
        protected Request(UserContext userContext) : base(userContext)
        {
        }

        public abstract Task Read();
    }
}
