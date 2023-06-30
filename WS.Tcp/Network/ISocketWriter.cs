namespace WS.Tcp.Network
{
    public interface ISocketWriter
    {   
        Task WriteByteAsync(byte value);
        Task WriteFloat(float population);
        Task WriteByteArrayAsync(byte[] value);
        Task WriteZeroByte(int count);
    }
}
