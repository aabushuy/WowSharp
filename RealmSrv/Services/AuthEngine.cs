using System.Collections;
using System.Numerics;
using System.Security.Cryptography;

namespace RealmSrv.Services
{
    internal class AuthEngine : IAuthEngine
    {
        private static readonly byte[] _n = new byte[] { 137, 75, 100, 94, 137, 225, 83, 91, 189, 173, 91, 139, 41, 6, 80, 83, 8, 1, 177, 142, 191, 191, 94, 143, 171, 60, 130, 135, 42, 62, 155, 183 };
        private static readonly byte[] _salt = new byte[] { 173, 208, 58, 49, 210, 113, 20, 70, 117, 242, 112, 126, 80, 38, 182, 210, 241, 134, 89, 153, 118, 2, 80, 170, 185, 69, 224, 158, 221, 42, 163, 69 };
        private static readonly byte[] _crcSalt = new byte[16];
        private static readonly Random _random = new();

        private readonly byte[] _b = new byte[20];
        private readonly byte[] _k = new byte[] { 3 };
        private readonly byte[] _g = new byte[] { 7 };

        private byte[] _a;        
        private byte[] _s;        
        private byte[] _u;
        private byte[] _username;
        private byte[] _ssHash;

        private BigInteger _bna;
        private BigInteger _bNb;
        private BigInteger _bnPublicB;
        private BigInteger _bNg;
        private BigInteger _bNk;
        private BigInteger _bNn;
        private BigInteger _bns;
        private BigInteger _bnu;
        private BigInteger _bNv;
        private BigInteger _bNx;
                
        public byte[] M1 { get; private set; }
        public byte[] M2 { get; private set; }

        public byte[] PublicB { get; private set; }
        public byte[] G => _g;
        public byte[] N => _n;
        public byte[] Salt => _salt;
        public byte[] CrcSalt => _crcSalt;

        static AuthEngine()
        {
            _random.NextBytes(_crcSalt);
        }

        public AuthEngine()
        {
            M2 = Array.Empty<byte>();
            PublicB = new byte[32];
        }
        
        public void CalculateX(byte[] username, byte[] pwHash)
        {
            _username = username;
            SHA1 algorithm1 = SHA1.Create();

            byte[] buffer5 = new byte[(_salt.Length + 20)];
            Buffer.BlockCopy(pwHash, 0, buffer5, _salt.Length, 20);
            Buffer.BlockCopy(_salt, 0, buffer5, 0, _salt.Length);
            
            byte[] buffer3 = algorithm1.ComputeHash(buffer5);
            Array.Reverse(buffer3);
            
            _bNx = new BigInteger(buffer3, isUnsigned: true, isBigEndian: true);
            Array.Reverse(_g);
            
            _bNg = new BigInteger(_g, isUnsigned: true, isBigEndian: true);
            Array.Reverse(_g);
            Array.Reverse(_k);
            
            _bNk = new BigInteger(_k, isUnsigned: true, isBigEndian: true);
            Array.Reverse(_k);
            Array.Reverse(_n);
            
            _bNn = new BigInteger(_n, isUnsigned: true, isBigEndian: true);
            Array.Reverse(_n);
            
            CalculateV();
        }

        public void CalculateU(byte[] a)
        {
            _a = a;
            SHA1 algorithm1 = SHA1.Create();
            var buffer1 = new byte[(a.Length + PublicB.Length)];

            Buffer.BlockCopy(a, 0, buffer1, 0, a.Length);
            Buffer.BlockCopy(PublicB, 0, buffer1, a.Length, PublicB.Length);

            _u = algorithm1.ComputeHash(buffer1);
            Array.Reverse(_u);
            _bnu = new BigInteger(_u, isUnsigned: true, isBigEndian: true);
            Array.Reverse(_u);

            Array.Reverse(a);
            _bna = new BigInteger(a, isUnsigned: true, isBigEndian: true);
            Array.Reverse(a);

            CalculateS();
        }

        public void CalculateM1()
        {
            SHA1 algorithm1 = SHA1.Create();
            
            byte[] nHash = algorithm1.ComputeHash(_n);
            byte[] gHash = algorithm1.ComputeHash(_g);
            byte[] userHash = algorithm1.ComputeHash(_username);
            byte[] ngHash = new byte[20];

            for (var i = 0; i <= 19; i++)
                ngHash[i] = (byte)(nHash[i] ^ gHash[i]);

            var temp = Concat(ngHash, userHash);
            temp = Concat(temp, _salt);
            temp = Concat(temp, _a);
            temp = Concat(temp, PublicB);
            temp = Concat(temp, _ssHash);
            
            M1 = algorithm1.ComputeHash(temp);
        }
        
        public void CalculateM2(byte[] m1Loc)
        {
            SHA1 algorithm1 = SHA1.Create();
            var buffer1 = new byte[(_a.Length + m1Loc.Length + _ssHash.Length)];

            Buffer.BlockCopy(_a, 0, buffer1, 0, _a.Length);
            Buffer.BlockCopy(m1Loc, 0, buffer1, _a.Length, m1Loc.Length);
            Buffer.BlockCopy(_ssHash, 0, buffer1, _a.Length + m1Loc.Length, _ssHash.Length);

            M2 = algorithm1.ComputeHash(buffer1);
        }

        private void CalculateB()
        {
            _random.NextBytes(_b);
            Array.Reverse(_b);
            _bNb = new BigInteger(_b, isUnsigned: true, isBigEndian: true);
            Array.Reverse(_b);

            BigInteger ptr1 = BigInteger.ModPow(_bNg, _bNb, _bNn);
            BigInteger ptr2 = _bNk * _bNv;
            BigInteger ptr3 = ptr1 + ptr2;

            _bnPublicB = ptr3 % _bNn;
            PublicB = _bnPublicB.ToByteArray(isUnsigned: true, isBigEndian: true);
            Array.Reverse(PublicB);
        }

        private void CalculateK()
        {
            SHA1 algorithm1 = SHA1.Create();
            ArrayList list1 = Split(_s);

            list1[0] = algorithm1.ComputeHash((byte[])list1[0]);
            list1[1] = algorithm1.ComputeHash((byte[])list1[1]);
            _ssHash = Combine((byte[])list1[0], (byte[])list1[1]);
        }

        private void CalculateS()
        {
            BigInteger ptr1 = BigInteger.ModPow(_bNv, _bnu, _bNn);
            BigInteger ptr2 = _bna * ptr1;

            _bns = BigInteger.ModPow(ptr2, _bNb, _bNn);
            _s = _bns.ToByteArray(isUnsigned: true, isBigEndian: true);
            Array.Reverse(_s);

            CalculateK();
        }

        private void CalculateV()
        {
            _bNv = BigInteger.ModPow(_bNg, _bNx, _bNn);
            CalculateB();
        }

        private static byte[] Combine(byte[] buffer1, byte[] buffer2)
        {
            if (buffer1.Length != buffer2.Length)
                throw new InvalidOperationException($"length of buffer1({buffer1.Length}) != buffer2({buffer2.Length})");

            var сombineBuffer = new byte[(buffer1.Length + buffer2.Length)];
            var сounter = 0;
            for (int i = 0, loopTo = сombineBuffer.Length - 1; i <= loopTo; i += 2)
            {
                сombineBuffer[i] = buffer1[сounter];
                сounter += 1;
            }

            сounter = 0;
            for (int i = 1, loopTo1 = сombineBuffer.Length - 1; i <= loopTo1; i += 2)
            {
                сombineBuffer[i] = buffer2[сounter];
                сounter += 1;
            }

            return сombineBuffer;
        }

        private static byte[] Concat(byte[] buffer1, byte[] buffer2)
        {
            var ConcatBuffer = new byte[(buffer1.Length + buffer2.Length)];

            Array.Copy(buffer1, ConcatBuffer, buffer1.Length);
            Array.Copy(buffer2, 0, ConcatBuffer, buffer1.Length, buffer2.Length);
            
            return ConcatBuffer;
        }

        private static ArrayList Split(byte[] byteBuffer)
        {
            var splitBuffer1 = new byte[(int)((byteBuffer.Length / 2d) - 1d + 1)];
            var splitBuffer2 = new byte[(int)((byteBuffer.Length / 2d) - 1d + 1)];
            
                     
            var counter = 0;
            for (int i = 0, loopTo = splitBuffer1.Length - 1; i <= loopTo; i++)
            {
                splitBuffer1[i] = byteBuffer[counter];
                counter += 2;
            }

            counter = 1;
            for (int i = 0, loopTo1 = splitBuffer2.Length - 1; i <= loopTo1; i++)
            {
                splitBuffer2[i] = byteBuffer[counter];
                counter += 2;
            }

            return new ArrayList()
            {
                splitBuffer1,
                splitBuffer2
            };
        }
    }
}