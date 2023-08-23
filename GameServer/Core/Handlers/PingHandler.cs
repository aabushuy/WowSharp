using GameServer.Core.Requests;
using GameServer.Core.Responses;
using GameServer.Enums;
using MediatR;
using System.Buffers.Binary;

namespace GameServer.Core.Handlers
{
    internal class PingHandler : IRequestHandler<PingRequest, PingResponse>
    {
        public Task<PingResponse> Handle(PingRequest request, CancellationToken cancellationToken)
        {
            PingResponse response = new(OperationCode.SMSG_PONG);

            uint value = BinaryPrimitives.ReadUInt32LittleEndian(request.Body[..4].Span);

            _ = BinaryPrimitives.ReadUInt32LittleEndian(request.Body.Slice(4, 4).Span);

            response.AddUIntLittleEndian(value);

            return Task.FromResult(response);
        }
    }
}
