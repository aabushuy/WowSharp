namespace Realm.Entity
{
    internal enum OperationCode : byte
    {
        AuthLogonChallenge = 0x0,
        AuthLogonProof = 0x1,
        AuthReconnectChallenge = 0x2,
        AuthReconnectProof = 0x3,
        AuthRealmList = 0x10,        
        XferInitiate = 0x30,
        XferData = 0x31
    }
}
