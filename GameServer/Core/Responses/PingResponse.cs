using GameServer.Enums;

namespace GameServer.Core.Responses
{
    internal class PingResponse : Response
    {
        public PingResponse(OperationCode operationCode) : base(operationCode)
        {
        }

        public override byte[] GetData()
        {
            byte[] data = new byte[8];
            byte[] header = GetHeader(6);

            Array.Copy(header, data, header.Length);
            Array.Copy(Body, 0, data, header.Length, Body.Length);

            return data;
        }
    }
}
