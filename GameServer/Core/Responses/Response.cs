using GameServer.Enums;
using System.Buffers.Binary;

namespace GameServer.Core.Responses
{
    internal abstract class Response
    {
        private readonly short _operationCodeShort;
        private int _offsetBody;
        private byte[] _body = Array.Empty<byte>();

        public byte[] Body => _body;

        public Response(OperationCode operationCode)
        {
            _operationCodeShort = (short)operationCode;
        }

        public abstract byte[] GetData();

        protected byte[] GetHeader(int messageLength)
        {
            byte[] messageLengthArray = new byte[2];
            BinaryPrimitives.WriteInt16BigEndian(messageLengthArray, (short)messageLength);

            byte[] operationCodeArray = new byte[2];
            BinaryPrimitives.WriteInt16LittleEndian(operationCodeArray, _operationCodeShort);

            byte[] headerData = new byte[4];
            
            Array.Copy(messageLengthArray, headerData, messageLengthArray.Length);
            Array.Copy(operationCodeArray, 0, headerData, messageLengthArray.Length, operationCodeArray.Length);

            return headerData;
        }

        public void AddByteLittleEndian(byte value)
        {
            ResizeBody(1);

            _body[_offsetBody] = value;
        }

        public void AddUIntLittleEndian(uint value)
        {
            ResizeBody(4);

            byte[] result = new byte[4];
            
            BinaryPrimitives.WriteUInt32LittleEndian(result, value);

            Array.Copy(result, 0 , _body, _offsetBody, result.Length);
        }

        private void ResizeBody(int addCapacity)
        {
            _offsetBody = _body.Length;

            Array.Resize(ref _body, _body.Length + addCapacity);
        }
    }
}
