using Domain.Entity.Account;
using Realm.Network;

namespace Realm.Entity.Responses
{
    internal class LogonChallengeResponse : Response
    {
        public byte[] B { get; init; } 
        public byte[] Generator { get; init; }
        public byte[] PrimeNumber { get; init; }
        public byte[] Salt { get; init; }        
        public byte[] VersionChallenge { get; init; }

        public override async Task Write(ISocketWriter socketWriter)
        {
            await socketWriter.WriteByteAsync((byte)OperationCode.AuthLogonChallenge);
            await socketWriter.WriteZeroByte(1);
            await socketWriter.WriteByteAsync((byte)AccountStates.LoginOK);

            await socketWriter.WriteByteArrayAsync(B);
            await socketWriter.WriteByteAsync((byte)Generator.Length);
            await socketWriter.WriteByteAsync(Generator.First());

            await socketWriter.WriteByteAsync(32);
            await socketWriter.WriteByteArrayAsync(PrimeNumber);

            await socketWriter.WriteByteArrayAsync(Salt);
            await socketWriter.WriteByteArrayAsync(VersionChallenge);
            await socketWriter.WriteZeroByte(1);
        }
    }
}


