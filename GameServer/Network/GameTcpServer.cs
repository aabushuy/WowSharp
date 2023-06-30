using WS.Tcp;
using WS.Tcp.Entity;

namespace GameServer.Network
{
    internal class GameTcpServer : TcpServer
    {
        public GameTcpServer(ILogger<TcpServer> logger) : base(logger)
        {
        }

        public override Task ProcessSession(TcpSession tcpSession, CancellationToken stoppingToken)
        {


            throw new NotImplementedException();
        }
    }
}
