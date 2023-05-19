using RealmApp;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
        .AddHostedService<Worker>()
        .AddServices()
        .AddLogging(logging =>
        logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
            options.IncludeScopes = false;
        }));
    })
    .Build();

await host.RunAsync();
