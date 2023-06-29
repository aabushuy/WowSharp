using MediatR;
using RealmSrv.Entity.Responses;

namespace RealmSrv.Entity.Requests
{
    internal class LogonChallengeRequest : Request, IRequest<LogonChallengeResponse>
    {
        public string AccountName { get; private set; }
        public int ClientBuild { get; private set; }

        public LogonChallengeRequest(UserSession userContext) : base(userContext)
        {
        }

        public override async Task Read()
        {
            await Session.SkipBytes(10);

            ClientBuild = await Session.ReadIntAsync();

            await Session.SkipBytes(20);

            var accountLength = await Session.ReadByteAsync();

            AccountName = await Session.ReadStringAsync(accountLength);
        }
    }
}
