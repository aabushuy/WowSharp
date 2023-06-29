using MediatR;
using RealmSrv.Entity.Responses;

namespace RealmSrv.Entity.Requests
{
    internal class LogonProofRequest : Request, IRequest<LogonProofResponse>
    {
        public byte[] A { get; private set; }
        public byte[] M1 { get; private set; }

        public LogonProofRequest(UserSession session) : base(session)
        {
            A = Array.Empty<byte>();
            M1 = Array.Empty<byte>();
        }

        public override async Task Read()
        {
            A = await Session.ReadByteArrayAsync(32);
            M1 = await Session.ReadByteArrayAsync(20);

            await Session.SkipBytes(22);
        }
    }
}
