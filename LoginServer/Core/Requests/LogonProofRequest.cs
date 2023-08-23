using MediatR;
using LoginServer.Core.Responses;
using LoginServer.Entity;

namespace LoginServer.Core.Requests
{
    internal class LogonProofRequest : Request, IRequest<LogonProofResponse>
    {
        public byte[] A { get; private set; }
        public byte[] M1 { get; private set; }

        public LogonProofRequest(UserContext session) : base(session)
        {
            A = Array.Empty<byte>();
            M1 = Array.Empty<byte>();
        }

        public override async Task Read()
        {
            A = await UserContext.Reader.ReadByteArrayAsync(32);
            M1 = await UserContext.Reader.ReadByteArrayAsync(20);

            await UserContext.Reader.SkipBytes(22);
        }
    }
}
