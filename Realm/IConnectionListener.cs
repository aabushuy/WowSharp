namespace Realm
{
    public interface IConnectionListener
    {
        Task AcceptConnections(string endPoint, CancellationToken cancellationToken);
    }
}
