using GameServer.Core.Responses;
using GameServer.Enums;
using MediatR;

namespace GameServer.Core.Requests
{
    internal class PingRequest : Request, IRequest<PingResponse>
    {
        public PingRequest(Memory<byte> requestBody)
            : base(OperationCode.CMSG_PING, requestBody)
        {
        }
    }
}
