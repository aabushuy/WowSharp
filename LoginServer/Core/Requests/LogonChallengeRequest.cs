using MediatR;
using LoginServer.Core.Responses;
using LoginServer.Entity;

namespace LoginServer.Core.Requests
{
    internal class LogonChallengeRequest : Request, IRequest<LogonChallengeResponse>
    {
        public string AccountName { get; private set; }
        public int ClientBuild { get; private set; }

        public LogonChallengeRequest(UserContext userContext) : base(userContext)
        {
        }

        public override async Task Read()
        {
            await UserContext.Reader.SkipBytes(10);

            ClientBuild = await UserContext.Reader.ReadIntAsync();

            await UserContext.Reader.SkipBytes(20);

            var accountLength = await UserContext.Reader.ReadByteAsync();

            AccountName = await UserContext.Reader.ReadStringAsync(accountLength);
        }
    }
}
