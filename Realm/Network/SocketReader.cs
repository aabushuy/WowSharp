using System.Buffers;
using System.Net.Sockets;
using System.Text;

namespace Realm.Network
{
    internal class SocketReader : ISocketReader
    {
        private readonly Socket _socket;
        private readonly CancellationToken _cancellationToken;
        private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;

        public SocketReader(Socket socket, CancellationToken cancellationToken)
        {
            _socket = socket;
            _cancellationToken = cancellationToken;
        }

        public async Task<byte[]> ReadByteArrayAsync(int length)
        {
            var buffer = new byte[length];
            
            await ReadAsync(buffer, length);

            return buffer;
        }

        public async Task<byte> ReadByteAsync()
        {
            var buffer = _arrayPool.Rent(1);
            try
            {
                await ReadAsync(buffer, 1);
                return buffer.First();
            }
            finally
            {
                _arrayPool.Return(buffer);
            }
        }

        public async Task<int> ReadIntAsync()
        {
            var buffer = _arrayPool.Rent(2);
            try
            {
                await ReadAsync(buffer, 2);
                return BitConverter.ToInt32(buffer);
            }
            finally
            {
                _arrayPool.Return(buffer);
            }
        }

        public async Task<string> ReadStringAsync(int length)
        {
            var buffer = _arrayPool.Rent(length);
            try
            {
                await ReadAsync(buffer, length);
                return Encoding.UTF8.GetString(buffer, 0, length);
            }
            finally
            {
                _arrayPool.Return(buffer);
            }
        }

        public async Task SkipBytes(int count)
        {
            var buffer = _arrayPool.Rent(count);

            await ReadAsync(buffer, count);

            _arrayPool.Return(buffer);
        }

        private async Task ReadAsync(byte[] buffer, int length)
        {
            if (length == 0)
            {
                return;
            }

            var number = await _socket.ReceiveAsync(buffer.AsMemory(0, length), SocketFlags.None, _cancellationToken);
            if (number != length)
            {
                throw new InvalidOperationException($"Unexpected read bytes number {number}(expected {length})");
            }
        }
    }
}
