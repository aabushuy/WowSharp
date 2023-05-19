using Realm.Network;

namespace Realm.Entity.Requests
{
    internal abstract class Request
    {
        public UserContext UserContext { get; internal set; }

        public abstract Task Read(ISocketReader socketReader);
    }
}
