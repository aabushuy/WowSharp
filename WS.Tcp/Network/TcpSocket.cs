using System.Buffers;
using System.Net.Sockets;

namespace WS.Tcp.Network
{
    internal class TcpSocket
    {
        protected readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;
        protected readonly Socket _socket;
        protected readonly CancellationToken _cancellationToken;

        public TcpSocket(Socket socket, CancellationToken cancellationToken)
        {
            _socket = socket;
            _cancellationToken = cancellationToken;
        }
    }
}
