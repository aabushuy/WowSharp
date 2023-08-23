using GameServer.Enums;
using MediatR;
using System.Buffers.Binary;
using System.Net.Security;
using System.Text;

namespace GameServer.Core.Requests
{
    internal abstract class Request
    {
        public OperationCode OperationCode { get; private init; }

        public Memory<byte> Body { get; private init; }

        public Request()
        {
            
        }

        public Request(OperationCode operationCode, Memory<byte> requestBody)
        {
            OperationCode = operationCode;
            Body = requestBody;
        }


        public string GetString(int index)
        {
            StringBuilder sb = new();

            while (true)
            {
                var c = (char)Body.Slice(index, 1).Span[0];
                if (c == '\0')
                    break;

                sb.Append(c);
                index++;
            }

            return sb.ToString();
        }

        public uint GetUInt32(int index)
            => BinaryPrimitives.ReadUInt32LittleEndian(Body.Slice(index, 4).Span);


        public byte[] GetArray(int index, int length)
        {
            byte[] result = Array.Empty<byte>();

            return result;
        }
    }
}
