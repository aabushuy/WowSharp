using MediatR;
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
                    var client = new UserContext(clientSocket, stoppingToken);

                    await RunUserProcessing(client, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogDebug(ex.StackTrace);
                }
            }
        }

        private async Task RunUserProcessing(UserContext userContext, CancellationToken cancellationToken)
        {
            while (userContext.IsAlive)
            {
                try
                {
                    await ProcessUserRequest(userContext, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }                

                Task.Delay(500, cancellationToken).Wait(cancellationToken);
            }
        }

        private async Task ProcessUserRequest(UserContext userContext, CancellationToken cancellationToken)
        {
            byte code = await userContext.ReadByteAsync();
            OperationCode operationCode = (OperationCode)code;

            Request request = CreateRequest(operationCode, userContext);

            _logger.LogDebug($"Start processing {operationCode}");

            try
            {
                await request.Read();

                var response = await _mediator.Send(request, cancellationToken)
                    ?? throw new InvalidOperationException("Empty response");

                await ((Response)response).Write();

                _logger.LogDebug($"Processed {operationCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }
        }

        private static Request CreateRequest(OperationCode operationCode, UserContext context) 
            => operationCode switch
        {
            OperationCode.AuthLogonChallenge => new LogonChallengeRequest(context),

            _ => throw new InvalidOperationException($"Unknown operation code {operationCode}")
        };
    }
}