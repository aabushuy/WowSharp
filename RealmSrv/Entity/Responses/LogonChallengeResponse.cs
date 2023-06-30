using RealmSrv.Enums;

namespace RealmSrv.Entity.Responses
{
    internal class LogonChallengeResponse : Response
    {
        public byte[] B { get; init; }
        public byte[] Generator { get; init; }
        public byte[] PrimeNumber { get; init; }
        public byte[] Salt { get; init; }
        public byte[] VersionChallenge { get; init; }

        public LogonChallengeResponse(UserContext userContext) : base(userContext)
        {
        }

        public override async Task Write()
        {
            //header
            await UserContext.Writer.WriteByteAsync((byte)OperationCode.AuthLogonChallenge);
            await UserContext.Writer.WriteZeroByte(1);
            await UserContext.Writer.WriteByteAsync((byte)AccountStates.LoginOK);
            //body
            await UserContext.Writer.WriteByteArrayAsync(B);
            await UserContext.Writer.WriteByteAsync((byte)Generator.Length);
            await UserContext.Writer.WriteByteAsync(Generator.First());

            await UserContext.Writer.WriteByteAsync(32);
            await UserContext.Writer.WriteByteArrayAsync(PrimeNumber);

            await UserContext.Writer.WriteByteArrayAsync(Salt);
            await UserContext.Writer.WriteByteArrayAsync(VersionChallenge);
            await UserContext.Writer.WriteZeroByte(1);
        }
    }
}
