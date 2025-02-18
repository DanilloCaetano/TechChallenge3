using Common.MessagingService;
using Common.MessagingService.QueuesConfig;
using Domain.Contact.Entity;
using Domain.Region.Entity;
using Domain.Region.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                        queue: QueueNames.RegionInsert,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    ulong tg = 1;
                    consumer.ReceivedAsync += async (model, ea) => 
                    {
                        tg = ea.DeliveryTag;
                        bool resultInsert = await Consumer_InsertReceivedAsync(model, ea);
                    };

                    //await channel.BasicConsumeAsync(
                    //    queue: QueueNames.RegionInsert,
                    //    autoAck: false,
                    //    consumer: consumer);

                    string consumerTag = await channel.BasicConsumeAsync(QueueNames.RegionInsert, false, consumer);
                    await channel.BasicAckAsync(tg, false);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Insert Worker error: {err}", ex.Message);
                }

                await Task.Delay(10000, stoppingToken);
            }
        }

        private async Task<bool> Consumer_InsertReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            bool result = true;
            try
            {
                using var scopeService = _serviceProvider.CreateScope();
                var regionRepository = scopeService.ServiceProvider.GetRequiredService<IRegionRepository>();

                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var entity = JsonSerializer.Deserialize<RegionEntity>(message);
                _logger.LogInformation("Insert Worker get entity: " + JsonSerializer.Serialize(entity));

                if (entity != null)
                {
                    await regionRepository.AddAsync(entity);
                    _logger.LogInformation("Insert Worker added entity: " + JsonSerializer.Serialize(entity));
                }
            }
            catch(Exception ex)
            {
                result = false;
                _logger.LogInformation("Insert Worker error: {err}", ex.Message);
            }

            return result;
        }
    }
}
