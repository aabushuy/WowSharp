using MediatR;
using Microsoft.Extensions.Logging;
using RealmSrv.Entity;
using RealmSrv.Entity.Requests;
using RealmSrv.Entity.Responses;
using RealmSrv.Enums;
using System.Net;
using System.Net.Sockets;

namespace RealmSrv
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string endPoint = _configuration["Realm:Endpoint"];

            IPEndPoint endPointParsed = IPEndPoint.Parse(endPoint)
                ?? throw new Exception($"Wrong Realm endpoint [{endPoint}]");

            TcpListener tcpServer = new (endPointParsed);
            tcpServer.Start();

            _logger.LogInformation($"Realm server started at {endPointParsed}");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Waiting a new client...");

                Socket clientSocket = await tcpServer.AcceptSocketAsync(stoppingToken);

                try
                {
                    var session = new UserSession(clientSocket, stoppingToken);

                    await RunUserProcessing(session, stoppingToken);
                }
                catch (SocketException exception) when (exception.SocketErrorCode == SocketError.ConnectionAborted)
                {
                    _logger.LogInformation("Connection aborted");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogDebug(ex.StackTrace);
                }

                _logger.LogInformation($"Connection closed {clientSocket.RemoteEndPoint}");
            }
        }

        private async Task RunUserProcessing(UserSession session, CancellationToken cancellationToken)
        {
            while (session.IsAlive)
            {
                try
                {
                    await ProcessUserRequest(session, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
        }

        private async Task ProcessUserRequest(UserSession session, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Waiting code");
            byte code = await session.ReadByteAsync();            
            _logger.LogInformation($"Start processing code: {code}");

            OperationCode operationCode = (OperationCode)code;
            _logger.LogDebug($"Operation: {operationCode}");

            Request request = CreateRequest(operationCode, session);

            try
            {
                await request.Read();

                var response = await _mediator.Send(request, cancellationToken)
                    ?? throw new InvalidOperationException("Empty response");

                await ((Response)response).Write();

                _logger.LogDebug($"Operation: {operationCode} completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }
        }

        private static Request CreateRequest(OperationCode operationCode, UserSession session) 
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