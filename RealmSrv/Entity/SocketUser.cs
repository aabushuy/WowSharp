using System.Buffers;
using System.Net.Sockets;
using System.Text;

namespace RealmSrv.Entity
{
    internal abstract class SocketUser
    {
        private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;

        protected readonly Socket _socket;
        protected readonly CancellationToken _cancellationToken;

        public SocketUser(Socket socket, CancellationToken cancellationToken)
        {
            _socket = socket;
            _cancellationToken = cancellationToken;
        }

        #region READ
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
        #endregion

        #region WRITE
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

        public async Task WriteFloat(float population)
        {
            var buffer = _arrayPool.Rent(sizeof(float));
            try
            {
                BitConverter.TryWriteBytes(buffer, population);
                await WriteAsync(buffer, sizeof(float));
            }
            finally
            {
                _arrayPool.Return(buffer);
            }
        }
        #endregion

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

        private async Task WriteAsync(byte[] buffer, int length)
        {
            await _socket.SendAsync(buffer.AsMemory(0, length), SocketFlags.None, _cancellationToken);
        }
    }
}
