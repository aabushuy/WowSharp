namespace Realm.Services
{
    public interface IAuthEngine
    {
        byte[] Generator { get; }
        byte[] PrimeNumber { get; }
        byte[] Salt { get; }
        byte[] VersionChallenge { get; }
        
        byte[] PublicB { get; }        
        byte[] M1 { get; }
        byte[] M2 { get; }


        void Init(string username, string passwordHashed);

        void CalculateSessionKey(byte[] a);
        void HashSessionKey();

        void CalculateM1(string userName);
        void CalculateM2();
    }
}
