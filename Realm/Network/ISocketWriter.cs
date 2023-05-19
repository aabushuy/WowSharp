namespace Realm.Network
{
    internal interface ISocketWriter
    {
        Task WriteByteAsync(byte value);
        Task WriteByteArrayAsync(byte[] value);
        Task WriteZeroByte(int count);
    }
}
