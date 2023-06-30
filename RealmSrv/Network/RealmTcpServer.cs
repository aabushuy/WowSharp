using MediatR;
using RealmSrv.Entity;
using RealmSrv.Entity.Requests;
using RealmSrv.Entity.Responses;
using RealmSrv.Enums;
using WS.Tcp;
using WS.Tcp.Entity;

namespace RealmSrv.Network
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

            try
            {
                _logger.LogDebug($"Operation: {operationCode}");

                await ProcessOperation(operationCode, userContext, stoppingToken);

                _logger.LogDebug($"Operation: {operationCode} completed");
            }
            catch (Exception ex)
            {
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

        private static Request CreateRequest(OperationCode operationCode, UserContext session)
            => operationCode switch
            {
                OperationCode.AuthLogonChallenge => new LogonChallengeRequest(session),
                OperationCode.AuthLogonProof => new LogonProofRequest(session),
                OperationCode.AuthReconnectChallenge => new LogonChallengeRequest(session),
                OperationCode.AuthReconnectProof => new LogonProofRequest(session),

                OperationCode.AuthRealmList => new RealmListRequest(session),

                _ => throw new InvalidOperationException($"Unknown operation code {operationCode}")
            };
    }
}
