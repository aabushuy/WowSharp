using WS.Tcp.Entity;

namespace WS.Tcp
{
    public interface ITcpServer
    {
        Task Run(string endPoint, CancellationToken token);

        Task ProcessSession(TcpSession tcpSession, CancellationToken stoppingToken);
    }
}
