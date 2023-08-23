using MediatR;
using LoginServer.Core.Responses;
using LoginServer.Entity;

namespace LoginServer.Core.Requests
{
    internal class RealmListRequest : Request, IRequest<RealmListResponse>
    {
        public byte[] Unk { get; private set; }

        public RealmListRequest(UserContext session) : base(session)
        {
        }

        public override async Task Read()
        {
            Unk = await UserContext.Reader.ReadByteArrayAsync(4);
        }
    }
}
