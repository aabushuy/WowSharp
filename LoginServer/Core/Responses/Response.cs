using LoginServer.Entity;

namespace LoginServer.Core.Responses
{
    internal abstract class Response : RequestResponse
    {
        protected Response(UserContext userContext) : base(userContext)
        {
        }

        public abstract Task Write();
    }
}
