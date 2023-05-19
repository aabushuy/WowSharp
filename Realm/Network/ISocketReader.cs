namespace Realm.Network
{
    internal interface ISocketReader
    {
        Task<byte> ReadByteAsync();
        Task<byte[]> ReadByteArrayAsync(int length);
        Task<int> ReadIntAsync();
        Task<string> ReadStringAsync(int length);
        Task SkipBytes(int count);

    }
}
