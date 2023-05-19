using Realm.Services;

Console.WriteLine("Start");

IAuthEngine _authEngine = new AuthEngine();

//qwer:123
var passwordHash = "BCC8A524726544C45375256397848DFDF4E19C1B";

_authEngine.CalculateB(passwordHash);

Console.WriteLine(_authEngine.PublicB);