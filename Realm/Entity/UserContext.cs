using Realm.Network;
using System.Net.Sockets;

namespace Realm.Entity
{
    internal class UserContext
    {
        public ISocketReader Reader { get; }
        public ISocketWriter Writer { get; }

        private readonly Dictionary<string, object> _parameters = new();

        public UserContext(Socket clientSocket, CancellationToken cancellationToken)
        {
            Reader = new SocketReader(clientSocket, cancellationToken);
            Writer = new SocketWriter(clientSocket, cancellationToken);
        }

        public object this[string index]
        {
            get => _parameters [index];
            set => _parameters[index] = value;
        }
    }
}
