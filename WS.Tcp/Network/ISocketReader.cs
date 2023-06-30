namespace WS.Tcp.Network
{
    public interface ISocketReader
    {
        Task<byte> ReadByteAsync();
        Task<int> ReadIntAsync();
        Task<byte[]> ReadByteArrayAsync(int length);        
        Task<string> ReadStringAsync(int length);
        Task SkipBytes(int count);
    }
}
