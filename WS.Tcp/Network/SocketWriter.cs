using System.Buffers.Binary;
using System.Net.Sockets;

namespace WS.Tcp.Network
{
    internal class SocketWriter : TcpSocket, ISocketWriter
    {
        public SocketWriter(Socket socket, CancellationToken cancellationToken)
            : base(socket, cancellationToken)
        {
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

        private async Task WriteAsync(byte[] buffer, int length)
        {
            await _socket.SendAsync(buffer.AsMemory(0, length), SocketFlags.None, _cancellationToken);
        }
    }
}
