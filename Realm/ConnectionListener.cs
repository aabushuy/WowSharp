using MediatR;
using Microsoft.Extensions.Logging;
using Realm.Entity;
using Realm.Entity.Requests;
using Realm.Entity.Responses;
using System.Net;
using System.Net.Sockets;

namespace Realm
{
    internal class ConnectionListener : IConnectionListener
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ConnectionListener> _logger;

        public ConnectionListener(IMediator mediator, ILogger<ConnectionListener> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task AcceptConnections(string endPoint, CancellationToken cancellationToken)
        {
            var endPointParsed = IPEndPoint.Parse(endPoint)
                ?? throw new Exception($"Wrong Realm endpoint [{endPoint}]");

            var tcpServer = new TcpListener(endPointParsed);

            tcpServer.Start();

            _logger.LogInformation($"Tcp server started at {endPointParsed}");

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Waiting a new client...");

                Socket clientSocket = await tcpServer.AcceptSocketAsync(cancellationToken);

                try
                {
                    await HandleTcpClient(clientSocket, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                }
                
            }
        }

        private async Task HandleTcpClient(Socket clientSocket, CancellationToken cancellationToken)
        {
            UserContext userContext = new (clientSocket, cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                var code = await userContext.Reader.ReadByteAsync();
                var operationCode = (OperationCode)code;
                _logger.LogDebug(operationCode.ToString());

                var request = CreateRequest(operationCode, userContext);

                await request.Read(userContext.Reader);

                var response = await _mediator.Send(request, cancellationToken);

                if (response == null)
                    throw new InvalidOperationException("Empty response");

                await ((Response)response).Write(userContext.Writer);

                Task.Delay(500, cancellationToken).Wait();
            }
        }

        private static Request CreateRequest(OperationCode operationCode, UserContext context)
        {
            Request request = operationCode switch
            {
                OperationCode.AuthLogonChallenge => new LogonChallengeRequest(),
                OperationCode.AuthLogonProof => new LogonProofRequest(),
                _ => throw new InvalidOperationException($"Unknown operation code {operationCode}")
            };

            request.UserContext = context;

            return request;
        }
    }
}
