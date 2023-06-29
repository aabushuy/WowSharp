using RealmSrv.Services;
using System.Net.Sockets;

namespace RealmSrv.Entity
{
    internal class UserSession : SocketUser
    {   
        public Account AccountInfo { get; set; }

        public IAuthEngine Auth { get; }

        public bool IsAlive => !_cancellationToken.IsCancellationRequested;

        public UserSession(Socket socket, CancellationToken cancellationToken)
            : base(socket, cancellationToken)
        {
            Auth = new AuthEngine();
        }
    }
}
