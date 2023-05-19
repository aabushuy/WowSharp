using Domain.Entity.Account;
using Realm.Network;

namespace Realm.Entity.Responses
{
    internal class LogonProofResponse : Response
    {
        public AccountStates AccountState { get; set; }

        public byte[] M2 { get; set; }

        public override async Task Write(ISocketWriter socketWriter)
        {
            await socketWriter.WriteByteAsync((byte)OperationCode.AuthLogonProof);
            await socketWriter.WriteByteAsync((byte)AccountState);

            if (M2 != null && M2.Length > 0)
            {
                await socketWriter.WriteByteArrayAsync(M2);
                await socketWriter.WriteZeroByte(4);
            }
        }
    }
}
