using MediatR;
using Realm.Entity.Responses;
using Realm.Network;

namespace Realm.Entity.Requests
{
    internal class LogonProofRequest : Request, IRequest<LogonProofResponse>
    {
        public byte[] A { get; set; }
        public byte[] M1 { get; set; }        
        public byte[] CRCHash { get; set; }
        public byte NumberOfKeys { get; set; }
        public byte SecurityFlags { get; set; }

        public override async Task Read(ISocketReader socketReader)
        {
            A = await socketReader.ReadByteArrayAsync(32);
            M1 = await socketReader.ReadByteArrayAsync(20);
            CRCHash = await socketReader.ReadByteArrayAsync(20);
            NumberOfKeys = await socketReader.ReadByteAsync();
            SecurityFlags = await socketReader.ReadByteAsync();
        }
    }
}
