using MediatR;
using LoginServer.Entity;
using WS.Tcp;
using WS.Tcp.Entity;
using LoginServer.Core.Responses;
using LoginServer.Core.Requests;

namespace LoginServer.Network
{
    internal class RealmTcpServer : TcpServer
    {
        private readonly ILogger<TcpServer> _logger;
        private readonly IMediator _mediator;

        public RealmTcpServer(ILogger<TcpServer> logger, IMediator mediator) : base(logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task ProcessSession(TcpSession tcpSession, CancellationToken stoppingToken)
        {
            var context = new UserContext(tcpSession);

            while (tcpSession.IsAlive || !stoppingToken.IsCancellationRequested)
            {
                await ProcessSessionRequest(context, stoppingToken);
            }
        }

        private async Task ProcessSessionRequest(UserContext userContext, CancellationToken stoppingToken)
        {
            byte code = await userContext.Reader.ReadByteAsync();

            OperationCode operationCode = (OperationCode)code;

            _logger.LogDebug($"Operation: {operationCode}");

            try
            {
                await ProcessOperation(operationCode, userContext, stoppingToken);

                _logger.LogDebug($"Operation: {operationCode} completed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Operation: {operationCode} failed");
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }
        }

        private async Task ProcessOperation(OperationCode operationCode, UserContext userContext, CancellationToken stoppingToken)
        {
            Request request = CreateRequest(operationCode, userContext);

            await request.Read();

            var response = await _mediator.Send(request, stoppingToken)
                ?? throw new InvalidOperationException("Empty response");

            await ((Response)response).Write();
        }

        private static Request CreateRequest(OperationCode operationCode, UserContext context)
            => operationCode switch
            {
                OperationCode.AuthLogonChallenge => new LogonChallengeRequest(context),
                OperationCode.AuthLogonProof => new LogonProofRequest(context),
                OperationCode.AuthReconnectChallenge => new LogonChallengeRequest(context),
                OperationCode.AuthReconnectProof => new LogonProofRequest(context),

                OperationCode.AuthRealmList => new RealmListRequest(context),

                _ => throw new InvalidOperationException($"Unknown operation code {operationCode}")
            };
    }
}
