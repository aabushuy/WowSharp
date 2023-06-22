using RealmSrv.Enums;

namespace RealmSrv.Entity.Responses
{
    internal class LogonChallengeResponse : Response
    {
        public byte[] B { get; set; }
        public byte[] Generator { get; set; }
        public byte[] PrimeNumber { get; init; }
        public byte[] Salt { get; set; }
        public byte[] VersionChallenge { get; init; }

        public LogonChallengeResponse(UserContext userContext) : base(userContext)
        {
        }

        public override async Task Write()
        {
            //header
            await _userContext.WriteByteAsync((byte)OperationCode.AuthLogonChallenge);
            await _userContext.WriteZeroByte(1);
            await _userContext.WriteByteAsync((byte)AccountStates.LoginOK);
            //body
            await _userContext.WriteByteArrayAsync(B);
            await _userContext.WriteByteAsync((byte)Generator.Length);
            await _userContext.WriteByteAsync(Generator.First());

            await _userContext.WriteByteAsync(32);
            await _userContext.WriteByteArrayAsync(PrimeNumber);

            await _userContext.WriteByteArrayAsync(Salt);
            await _userContext.WriteByteArrayAsync(VersionChallenge);
            await _userContext.WriteZeroByte(1);

            await _userContext.WriteZeroByte(20);
        }
    }
}
