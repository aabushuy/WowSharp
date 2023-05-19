using Realm;

namespace RealmApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnectionListener _connectionListener;
        private readonly IConfiguration _configuration;

        public Worker(
            ILogger<Worker> logger,
            IConnectionListener connectionListener,
            IConfiguration configuration)
        {
            _logger = logger;
            _connectionListener = connectionListener;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting realm server at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                var endPoint = _configuration["Realm:Endpoint"];

                await _connectionListener.AcceptConnections(endPoint, stoppingToken);
            }

            _logger.LogInformation("Stopping realm server at: {time}", DateTimeOffset.Now);
        }
    }
}