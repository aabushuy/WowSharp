using LoginServer.Entity;

namespace LoginServer.Core.Responses
{
    internal class LogonProofResponse : Response
    {
        private readonly byte[] _accountFlag = { 0x0, 0x8, 0x0, 0x0 };

        public AccountStates AccountState { get; init; }

        public byte[] M2 { get; init; }

        public LogonProofResponse(UserContext userContext) : base(userContext)
        {
            M2 = Array.Empty<byte>();
        }

        public override async Task Write()
        {
            await UserContext.Writer.WriteByteAsync((byte)OperationCode.AuthLogonProof);
            await UserContext.Writer.WriteByteAsync((byte)AccountState);

            await UserContext.Writer.WriteByteArrayAsync(M2);
            await UserContext.Writer.WriteByteArrayAsync(_accountFlag);
            await UserContext.Writer.WriteZeroByte(6);
        }
    }
}

