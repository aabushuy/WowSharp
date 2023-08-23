using LoginServer.Services;
using WS.Tcp.Entity;
using WS.Tcp.Network;

namespace LoginServer.Entity
{
    internal class UserContext
    {
        public ISocketReader Reader { get; }
        public ISocketWriter Writer { get; }        
        public Account AccountInfo { get; set; }
        public IAuthEngine Auth { get; }
        public UserContext(TcpSession tcpSession)
        {
            Auth = new AuthEngine();
            Reader = tcpSession.Reader;
            Writer = tcpSession.Writer;
        }
    }
}
