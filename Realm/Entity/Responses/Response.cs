using Realm.Network;

namespace Realm.Entity.Responses
{
    internal abstract class Response
    {
        public abstract Task Write(ISocketWriter socketWriter);
    }
}
