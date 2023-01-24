using Microsoft.AspNetCore.SignalR.Client;

namespace Voice_Server.Background
{
    public class Socket_Test_Background : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => ExecuteAsync(new CancellationToken()), cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                 Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                var connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5063/voiceHub")
                    .Build();
                connection.On<string>("ReceiveMessage", Console.WriteLine);
                 connection.StartAsync(stoppingToken).WaitAsync(stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}