using System.Net.Sockets;
using WS.Tcp.Network;

namespace WS.Tcp.Entity
{
    public class TcpSession
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Socket _socket;

        public bool IsAlive => !_cancellationTokenSource.Token.IsCancellationRequested;

        public ISocketReader Reader { get; }
        public ISocketWriter Writer { get; }

        public byte[] Key { get; } = new byte[4];
        public byte[] Hash { get; } = new byte[40];
        public bool IsEncrypt { get; set; } = false;

        public TcpSession(Socket socket, CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _socket = socket;

            Reader = new SocketReader(_socket, _cancellationTokenSource.Token);
            Writer = new SocketWriter(_socket, _cancellationTokenSource.Token);
        }

        public void CloseSession()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
