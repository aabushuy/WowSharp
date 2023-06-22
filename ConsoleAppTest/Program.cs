using ConsoleAppTest;

Console.WriteLine("Start");

UserManager userManager = new UserManager();

//qwer:123
var passwordHash = "BCC8A524726544C45375256397848DFDF4E19C1B";

var username = "qwer";
var password = "123";

userManager.CreateUser(username, password);


Console.WriteLine("Stop");