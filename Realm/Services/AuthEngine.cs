using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Realm.Services
{
    public class AuthEngine : IAuthEngine
    {
        private static readonly byte[] _primeNumber = new byte[] { 137, 75, 100, 94, 137, 225, 83, 91, 189, 173, 91, 139, 41, 6, 80, 83, 8, 1, 177, 142, 191, 191, 94, 143, 171, 60, 130, 135, 42, 62, 155, 183 };
        private static readonly byte[] _saltArray = new byte[] { 173, 208, 58, 49, 210, 113, 20, 70, 117, 242, 112, 126, 80, 38, 182, 210, 241, 134, 89, 153, 118, 2, 80, 170, 185, 69, 224, 158, 221, 42, 163, 69 };
        private static readonly byte[] _versionChallenge = { 0xBA, 0xA3, 0x1E, 0x99, 0xA0, 0x0B, 0x21, 0x57, 0xFC, 0x37, 0x3F, 0xB3, 0x69, 0xCD, 0xD2, 0xF1 };
        private static readonly byte[] _generator = new byte[] { 7 };
        private static readonly byte[] _bArray = new byte[] { 236, 199, 129, 66, 111, 90, 63, 52, 126, 229, 224, 244, 140, 215, 165, 249, 29, 185, 155, 114 };
        
        //OUT
        public byte[] Generator => _generator;
        public byte[] PrimeNumber => _primeNumber;
        public byte[] Salt => _saltArray;
        public byte[] VersionChallenge => _versionChallenge;
        public byte[] PublicB { get; private set; } = Array.Empty<byte>();
        public byte[] M1 { get; private set; } = Array.Empty<byte>();
        public byte[] M2 { get; private set; } = Array.Empty<byte>();

        //LOCAL
        private BigInteger _g; // generator
        private BigInteger _b; // random bytes
        private BigInteger _N; // prime number

        private BigInteger _s; // salt

        private BigInteger _x; // password hash
        private BigInteger _v; // virifier

        private BigInteger _A; // from Client
        private BigInteger _S; // session key
        private BigInteger _K; // key

        public AuthEngine()
        {
            _g = GetBigInteger(_generator);
            _b = GetBigInteger(_bArray);
            _N = GetBigInteger(_primeNumber);

            _s = GetBigInteger(_saltArray);
        }

        public void CalculateB(string passwordHash)
        {
            byte[] passwordHashArray = GetPasswordHashFromString(passwordHash);
            byte[] passwordHashAndSalt = _saltArray.Concat(passwordHashArray).ToArray();
            byte[] passwordHashSha1 = SHA1.HashData(passwordHashAndSalt);

            _x = GetBigInteger(passwordHashSha1);
            _v = BigInteger.ModPow(_g, _x, _N);

            BigInteger gmod = BigInteger.ModPow(_g, _b, _N);
            BigInteger b = ((_v * 3) + gmod) % _N;

            PublicB = b.ToByteArray(isUnsigned: true, isBigEndian: true);
        }

        public void CalculateSessionKey(byte[] a)
        {
            if (PublicB.Length == 0)
                throw new InvalidOperationException($"PublicB is empty");
            
            _A = GetBigInteger(a);

            if(_A == 0 || _A % _N == 0)
                throw new InvalidOperationException($"Wrong request value [A] or [N]");

            var buffer = _A.ToByteArray()
                .Concat(_b.ToByteArray())
                .ToArray();

            BigInteger u = GetBigInteger(SHA1.HashData(buffer));

            var s = _A * BigInteger.ModPow(_v, u, _N);
            
            _S = BigInteger.ModPow(s, _b, _N);
        }

        public void HashSessionKey()
        {
            byte[] t = new byte[32];
            byte[] t1 = new byte[16];
            byte[] vK = new byte[40];

            var sArray = _S.ToByteArray();
            Array.Copy(sArray, t, sArray.Length);

            for (int i = 0; i < 16; ++i)
                t1[i] = t[i * 2];

            var t1Hash1 = SHA1.HashData(t1);

            for (int i = 0; i < 20; ++i)
                vK[i * 2] = t1Hash1[i];

            for (int i = 0; i < 16; ++i)
                t1[i] = t[i * 2 + 1];

            var t1Hash2 = SHA1.HashData(t1);

            for (int i = 0; i < 20; ++i)
                vK[i * 2 + 1] = t1Hash2[i];

            _K = GetBigInteger(vK);
        }

        public void CalculateM1(string userName)
        {
            var hash = SHA1.HashData(_N.ToByteArray());
            var gHash = SHA1.HashData(_g.ToByteArray());

            for (var i = 0; i < hash.Length; i++)
                hash[i] = (byte)(hash[i] ^ gHash[i]);

            byte[] usernameHashed = SHA1.HashData(Encoding.UTF8.GetBytes(userName));

            var result = GetBigInteger(hash).ToByteArray()
                .Concat(usernameHashed)
                .Concat(_s.ToByteArray())
                .Concat(_A.ToByteArray())
                .Concat(PublicB)
                .Concat(_K.ToByteArray())
                .ToArray();

            M1 = SHA1.HashData(result);
        }

        public void CalculateM2()
        {
            var result = _A.ToByteArray()
                .Concat(M1)
                .ToArray();

            M2 = SHA1.HashData(result);
        }

        private static BigInteger GetReversedBigInteger(byte[] input)
        {
            Array.Reverse(input);

            BigInteger bNn = GetBigInteger(input);

            Array.Reverse(input);

            return bNn;
        }

        private static BigInteger GetBigInteger(byte[] input) => new(input, isUnsigned: true, isBigEndian: true);

        private static byte[] GetPasswordHashFromString(string shaPassword)
        {
            var hash = new byte[20];
            for (var i = 0; i < 40; i += 2)
            {
                hash[i / 2] = byte.Parse(shaPassword.AsSpan(i, 2), NumberStyles.HexNumber);
            }
            return hash;
        }

        
    }
}