using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using WS.Tcp.Entity;

namespace WS.Tcp
{
    public abstract class TcpServer : ITcpServer
    {
        private readonly ILogger<TcpServer> _logger;

        public TcpServer(ILogger<TcpServer> logger)
        {
            _logger = logger;
        }

        public async Task Run(string endPoint, CancellationToken token)
        {
            var tcpServer = CreateTcpListener(endPoint);
            
            tcpServer.Start();

            _logger.LogInformation($"Server started at {endPoint}");

            while (!token.IsCancellationRequested) 
            {
                _logger.LogInformation("Waiting a new client...");

                Socket clientSocket = await tcpServer.AcceptSocketAsync(token);

                await ProcessClientSocket(clientSocket, token);

                _logger.LogInformation($"Connection closed {clientSocket.RemoteEndPoint}");
            }
        }

        public abstract Task ProcessSession(TcpSession tcpSession, CancellationToken stoppingToken);

        private async Task ProcessClientSocket(Socket clientSocket, CancellationToken stoppingToken)
        {
            try
            {
                TcpSession session = new(clientSocket, stoppingToken);

                await ProcessSession(session, stoppingToken);
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
        }

        private static TcpListener CreateTcpListener(string endPoint)
        {
            IPEndPoint endPointParsed = IPEndPoint.Parse(endPoint)
                ?? throw new Exception($"Wrong endpoint [{endPoint}]");

            return new TcpListener(endPointParsed);
        }
    }
}