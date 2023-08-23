using GameServer.Core.Requests;
using GameServer.Core.Responses;
using GameServer.Enums;
using GameServer.Factory;
using MediatR;
using System.Buffers;
using System.Buffers.Binary;
using WS.Tcp;
using WS.Tcp.Entity;
using WS.Tcp.Network;

namespace GameServer.Network
{
    internal class GameTcpServer : TcpServer
    {
        private const int MAX_PACKET_LENGTH = 10000;
        private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;
        private readonly IMediator _mediator;
        private readonly IRequestFactory _requestFactory;

        public GameTcpServer(IMediator mediator, IRequestFactory requestBuilder, ILogger<TcpServer> logger) : base(logger)
        {
            _mediator = mediator;
            _requestFactory = requestBuilder;
        }

        public override async Task ProcessSession(TcpSession tcpSession, CancellationToken stoppingToken)
        {
            await HandleConnection(tcpSession);

            while (!stoppingToken.IsCancellationRequested && tcpSession.IsAlive)
            {
                await HandlePackageAsync(tcpSession);
            }

            _logger.LogDebug("Lost connection");
        }

        private async Task HandleConnection(TcpSession tcpSession)
        {
            _logger.LogDebug("Start client conversation");

            Response response = new AuthResponse(OperationCode.SMSG_AUTH_CHALLENGE);

            response.AddUIntLittleEndian(1);

            await SendResponseToClient(response, tcpSession);
        }

        private async Task HandlePackageAsync(TcpSession tcpSession)
        {
            using var memoryOwner = _memoryPool.Rent(MAX_PACKET_LENGTH);

            try
            {
                Memory<byte> header = await ReadHeaderAsync(tcpSession.Reader, memoryOwner.Memory);
                Memory<byte> body = await ReadBodyAsync(tcpSession.Reader, memoryOwner.Memory);

                uint operationCode = BinaryPrimitives.ReadUInt32LittleEndian(header.Span[2..]);

                _logger.LogDebug("Process code:{0}", operationCode);

                OperationCode opcode = (OperationCode)operationCode;

                Request request = _requestFactory.Create(opcode, body, tcpSession);

                var response = await _mediator.Send(request)
                    ?? throw new InvalidOperationException("Empty response");

                await SendResponseToClient((Response)response, tcpSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                tcpSession.CloseSession();
            }
        }

        private static async Task<Memory<byte>> ReadHeaderAsync(ISocketReader reader, Memory<byte> buffer)
        {
            var header = buffer[..6];

            await reader.ReadByteArrayAsync(header);

            return header;
        }

        private static async Task<Memory<byte>> ReadBodyAsync(ISocketReader reader, Memory<byte> buffer)
        {
            var length = BinaryPrimitives.ReadUInt16BigEndian(buffer.Span) - 4;

            var body = buffer.Slice(6, length);

            await reader.ReadByteArrayAsync(body);

            return body;
        }

        private static async Task SendResponseToClient(Response response, TcpSession tcpSession)
        {
            byte[] data = response.GetData();

            if (tcpSession.IsEncrypt)
            {
                Encode(data, tcpSession.Key, tcpSession.Hash);
            }

            await tcpSession.Writer.WriteByteArrayAsync(data);
        }

        public static void Encode(byte[] data, byte[] key, byte[] hash)
        {
            for (var i = 0; i < 4; i++)
            {
                data[i] = (byte)(((hash[key[3]] ^ data[i]) + key[2]) % 256);
                key[2] = data[i];
                key[3] = (byte)((key[3] + 1) % 40);
            }
        }
    }
}
