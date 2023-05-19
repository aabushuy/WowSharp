using System.Net.Sockets;
using System.Buffers;

namespace Realm.Network
{
    internal class SocketWriter : ISocketWriter
    {
        private readonly Socket _socket;
        private readonly CancellationToken _cancellationToken;
        private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;

        public SocketWriter(Socket socket, CancellationToken cancellationToken)
        {
            _socket = socket;
            _cancellationToken = cancellationToken;
        }

        public async Task WriteByteArrayAsync(byte[] value)
        {
            await WriteAsync(value, value.Length);
        }

        public async Task WriteByteAsync(byte value)
        {
            var buffer = _arrayPool.Rent(sizeof(byte));
            try
            {
                buffer[0] = value;
                await WriteAsync(buffer, sizeof(byte));
            }
            finally
            {
                _arrayPool.Return(buffer);
            }
        }

        public async Task WriteZeroByte(int count)
        {
            var buffer = _arrayPool.Rent(count);
            try
            {
                Array.Fill<byte>(buffer, 0);
                await WriteAsync(buffer, count);
            }
            finally
            {
                _arrayPool.Return(buffer);
            }
        }

        private async Task WriteAsync(byte[] buffer, int length)
        {
            await _socket.SendAsync(buffer.AsMemory(0, length), SocketFlags.None, _cancellationToken);
        }
    }
}
