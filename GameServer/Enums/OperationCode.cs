namespace GameServer.Enums
{
    internal enum OperationCode : short
    {
        CMSG_PING = 0x1DC,
        SMSG_PONG = 0x1DD,

        SMSG_AUTH_CHALLENGE = 0x1EC,
        CMSG_AUTH_SESSION = 0x1ED,
        SMSG_AUTH_RESPONSE = 0x1EE
    }
}
