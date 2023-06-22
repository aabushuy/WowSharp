using System.Security.Cryptography;
using SecureRemotePassword;

namespace ConsoleAppTest
{
    internal class UserManager
    {
        public void CreateUser(string username, string password)
        {
            var client = new SrpClient(GetParams());
            var salt = client.GenerateSalt();

            var privateKey = client.DerivePrivateKey(salt, username, password);
            var verifier = client.DeriveVerifier(privateKey);

            Console.WriteLine("PRIVATE KEY:");
            Console.WriteLine(privateKey);

            Console.WriteLine("VERIFIER");
            Console.WriteLine(verifier);


            var clientEphemeral = client.GenerateEphemeral();
        }

        private SrpParameters GetParams() 
        {
            byte[] N = new byte[] { 137, 75, 100, 94, 137, 225, 83, 91, 189, 173, 91, 139, 41, 6, 80, 83, 8, 1, 177, 142, 191, 191, 94, 143, 171, 60, 130, 135, 42, 62, 155, 183 };

            //"894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7"
            string Nhex = BitConverter.ToString(N).Replace("-", "");
            
            return SrpParameters.Create<SHA1>(Nhex, "7");
        }
    }
}
