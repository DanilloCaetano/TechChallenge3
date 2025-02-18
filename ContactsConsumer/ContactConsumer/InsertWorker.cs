using Common.MessagingService;
using Common.MessagingService.QueuesConfig;
using Domain.Contact.Entity;
using Domain.Contact.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ContactConsumer
{
    public class InsertWorker : BackgroundService
    {
        private readonly ILogger<InsertWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public InsertWorker(ILogger<InsertWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Insert Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    using var scopeService = _serviceProvider.CreateScope();
                    var _rabbitMqService = scopeService.ServiceProvider.GetRequiredService<IRabbitMqService>();

                    using var conn = await _rabbitMqService.GetConnection("rabbitmq-service.default.svc.cluster.local", "guest", "guest");
                    using var channel = await conn.CreateChannelAsync();

                    await channel.QueueDeclareAsync(
                        queue: QueueNames.ContactInsert,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += Consumer_InsertReceivedAsync;

                    await channel.BasicConsumeAsync(
                        queue: QueueNames.ContactInsert,
                        autoAck: true,
                        consumer: consumer,
                        noLocal: false,
                        exclusive: false,
                        consumerTag: string.Empty,
                        arguments: null);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Insert Worker error: {err}", ex.Message);
                }

                await Task.Delay(2500, stoppingToken);
            }
        }

        private async Task Consumer_InsertReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                using var scopeService = _serviceProvider.CreateScope();
                var _contactsService = scopeService.ServiceProvider.GetRequiredService<IContactRepository>();

                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var entity = JsonSerializer.Deserialize<ContactEntity>(message);
                _logger.LogInformation("Insert Worker get entity: " + JsonSerializer.Serialize(entity));

                if (entity != null)
                {
                    await _contactsService.AddAsync(entity);
                    _logger.LogInformation("Insert Worker added entity: " + JsonSerializer.Serialize(entity));
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Insert Worker error: {err}", ex.Message);
            }
            
        }
    }
}
