using System.Net.Sockets;

namespace RealmSrv.Entity
{
    internal class UserContext : SocketUser
    {   
        public Account AccountInfo { get; set; }

        public bool IsAlive => !_cancellationToken.IsCancellationRequested;

        public UserContext(Socket socket, CancellationToken cancellationToken): base(socket, cancellationToken) { }
    }
}
