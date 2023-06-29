using RealmSrv.Enums;

namespace RealmSrv.Entity.Responses
{
    internal class LogonProofResponse : Response
    {
        private readonly byte[] _accountFlag = { 0x0, 0x8, 0x0, 0x0 };

        public AccountStates AccountState { get; init; }

        public byte[] M2 { get; init; }

        public LogonProofResponse(UserSession userContext) : base(userContext)
        {
            M2 = Array.Empty<byte>();
        }

        public override async Task Write()
        {
            await Session.WriteByteAsync((byte)OperationCode.AuthLogonProof);
            await Session.WriteByteAsync((byte)AccountState);

            await Session.WriteByteArrayAsync(M2);
            await Session.WriteByteArrayAsync(_accountFlag);
            await Session.WriteZeroByte(6);
        }
    }
}

