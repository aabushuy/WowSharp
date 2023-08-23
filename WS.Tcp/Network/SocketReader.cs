using System.Net.Sockets;
using System.Text;

namespace WS.Tcp.Network
{
    internal class SocketReader : TcpSocket, ISocketReader
    {
        public SocketReader(Socket socket, CancellationToken cancellationToken) : base(socket, cancellationToken)
        {
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

        public async Task<byte[]> ReadByteArrayAsync(int length)
        {
            var buffer = new byte[length];

            await ReadAsync(buffer, length);

            return buffer;
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

        public async Task ReadByteArrayAsync(Memory<byte> buffer)
        {
            await ReadAsync(buffer, buffer.Length);
        }

        private async Task ReadAsync(byte[] buffer, int length)
        {
            await ReadAsync(buffer.AsMemory(0, length), length);
        }

        private async Task ReadAsync(Memory<byte> buffer, int length)
        {
            if (length == 0)
            {
                return;
            }

            var number = await _socket.ReceiveAsync(buffer, SocketFlags.None, _cancellationToken);
            if (number != length)
            {
                throw new InvalidOperationException($"Unexpected read bytes number {number}(expected {length})");
            }
        }
    }
}
