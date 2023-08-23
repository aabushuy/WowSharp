using GameServer.Core.Requests;
using GameServer.Enums;
using WS.Tcp.Entity;

namespace GameServer.Factory
{
    internal class RequestFactory : IRequestFactory
    {

        public Request Create(OperationCode operationCode, Memory<byte> requestBody, TcpSession tcpSession)
        {
            return operationCode switch 
            {
                OperationCode.CMSG_PING => new PingRequest(requestBody),

                OperationCode.CMSG_AUTH_SESSION => new AuthRequest(operationCode, requestBody, tcpSession),                

                _ => throw new InvalidOperationException($"No mapping for {operationCode}")
            };;
        }
    }
}
