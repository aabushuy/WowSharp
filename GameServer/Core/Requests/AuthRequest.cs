using GameServer.Core.Responses;
using GameServer.Enums;
using MediatR;
using WS.Tcp.Entity;

namespace GameServer.Core.Requests
{
    internal class AuthRequest: Request, IRequest<AuthResponse>
    {
        public TcpSession Session { get; }

        public AuthRequest(OperationCode operationCode, Memory<byte> requestBody, TcpSession tcpSession)
            : base(operationCode, requestBody)
        {
            Session = tcpSession;
        }
    }
}
