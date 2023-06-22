namespace RealmSrv.Enums
{
    public enum AccountStates : byte
    {
        LoginOK = 0x0,
        LoginFailed = 0x1,
        LoginBaned = 0x3,

        LoginIncorrectPassword = 0x5,
    }
}
