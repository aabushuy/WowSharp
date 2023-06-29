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

        public LogonChallengeResponse(UserSession userContext) : base(userContext)
        {
        }

        public override async Task Write()
        {
            //header
            await Session.WriteByteAsync((byte)OperationCode.AuthLogonChallenge);
            await Session.WriteZeroByte(1);
            await Session.WriteByteAsync((byte)AccountStates.LoginOK);
            //body
            await Session.WriteByteArrayAsync(B);
            await Session.WriteByteAsync((byte)Generator.Length);
            await Session.WriteByteAsync(Generator.First());

            await Session.WriteByteAsync(32);
            await Session.WriteByteArrayAsync(PrimeNumber);

            await Session.WriteByteArrayAsync(Salt);
            await Session.WriteByteArrayAsync(VersionChallenge);
            await Session.WriteZeroByte(1);
        }
    }
}
