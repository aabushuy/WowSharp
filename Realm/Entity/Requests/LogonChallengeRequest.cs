using MediatR;
using Microsoft.Extensions.Logging;
using Realm.Entity.Responses;
using Realm.Network;

namespace Realm.Entity.Requests
{
    internal class LogonChallengeRequest : Request, IRequest<LogonChallengeResponse>
    {
        public string AccountName { get; set; }

        public int ClientBuild { get; set; }

        public override async Task Read(ISocketReader socketReader)
        {
            await socketReader.SkipBytes(10);

            ClientBuild = await socketReader.ReadIntAsync();

            await socketReader.SkipBytes(20);

            var accountLength = await socketReader.ReadByteAsync();
            
            AccountName = await socketReader.ReadStringAsync(accountLength);

            //6 +2 +20 + 1 + accountLength
        }
    }
}
