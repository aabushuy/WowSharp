using GameServer.Enums;

namespace GameServer.Core.Responses
{
    internal class AuthResponse : Response
    {
        public AuthResponse(OperationCode operationCode)
            : base(operationCode)
        {
        }

        public override byte[] GetData()
        {
            byte[] header = GetHeader(6);

            byte[] data = new byte[header.Length + Body.Length];

            Array.Copy(header, data, header.Length);
            Array.Copy(Body, 0, data, header.Length, Body.Length);

            return data;
        }
    }
}
