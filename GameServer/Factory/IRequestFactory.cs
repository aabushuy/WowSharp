using GameServer.Core.Requests;
using GameServer.Enums;
using WS.Tcp.Entity;

namespace GameServer.Factory
{
    internal interface IRequestFactory
    {
        Request Create(OperationCode operationCode, Memory<byte> requestBody, TcpSession tcpSession);
    }
}
