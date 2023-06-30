using System.Net.Sockets;
using WS.Tcp.Network;

namespace WS.Tcp.Entity
{
    public class TcpSession
    {
        private readonly CancellationToken _cancellationToken;
        public bool IsAlive => !_cancellationToken.IsCancellationRequested;
        public ISocketReader Reader { get; }
        public ISocketWriter Writer { get; }

        public TcpSession(Socket socket, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            Reader = new SocketReader(socket, _cancellationToken);
            Writer = new SocketWriter(socket, _cancellationToken);
        }
    }
}
