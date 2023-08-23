using WS.Tcp;

namespace LoginServer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITcpServer _tcpServer;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, ITcpServer tcpServer, IConfiguration configuration)
        {
            _logger = logger;
            _tcpServer = tcpServer;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string endPoint = _configuration["RealmInstace:Endpoint"];
            try
            {
                await _tcpServer.Run(endPoint, stoppingToken);

                _logger.LogInformation($"Realm server started at {endPoint}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }

            _logger.LogInformation($"Realm server stopped");
        }
    }
}