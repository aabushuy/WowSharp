namespace LoginServer.Services
{
    public interface IAuthEngine
    {
        byte[] PublicB { get; }
        byte[] G { get; }
        byte[] N { get; }
        byte[] Salt { get; }
        byte[] CrcSalt { get; }

        byte[] M1 { get; }
        byte[] M2 { get; }

        void CalculateX(byte[] username, byte[] pwHash);
        void CalculateU(byte[] a);
        void CalculateM1();
        void CalculateM2(byte[] m1Loc);
    }
}
