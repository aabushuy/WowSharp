using RealmSrv.Enums;

namespace RealmSrv.Entity.Responses
{
    internal class LogonProofResponse : Response
    {
        private readonly byte[] _accountFlag = { 0x0, 0x8, 0x0, 0x0 };

        public AccountStates AccountState { get; set; }
        public byte[] M2 { get; set; }

        public LogonProofResponse(UserSession userContext) : base(userContext)
        {
            M2 = Array.Empty<byte>();
        }

        public override async Task Write()
        {
            await _userContext.WriteByteAsync((byte)OperationCode.AuthLogonProof);
            await _userContext.WriteByteAsync((byte)AccountState);

            await _userContext.WriteByteArrayAsync(M2);
            await _userContext.WriteByteArrayAsync(_accountFlag);
            await _userContext.WriteZeroByte(6);
        }
    }
}

