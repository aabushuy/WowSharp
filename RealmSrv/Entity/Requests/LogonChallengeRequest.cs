using MediatR;
using RealmSrv.Entity.Responses;

namespace RealmSrv.Entity.Requests
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
            await User.SkipBytes(10);

            ClientBuild = await User.ReadIntAsync();

            await User.SkipBytes(20);

            var accountLength = await User.ReadByteAsync();

            AccountName = await User.ReadStringAsync(accountLength);
        }
    }
}
