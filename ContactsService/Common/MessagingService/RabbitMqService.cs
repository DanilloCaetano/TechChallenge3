using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.MessagingService
{
    public class RabbitMqService : IRabbitMqService
    {
        public async Task<IConnection> GetConnection(string hostName, string user, string pass)
        {
            // User and pass in parameters, beacuse is educational project
            var factory = new ConnectionFactory() { HostName = hostName, UserName = user, Password = pass, Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            return connection;
        }

        public async Task<bool> SendMessage<T>(string queueName, string routingKey, T bodyMessage)
        {
            try
            {
                using var conn = await GetConnection("rabbitmq-service.default.svc.cluster.local", "guest", "guest");
                using var channel = await conn.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonSerializer.Serialize(bodyMessage);
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: queueName,
                    body: body);

                return true;
            } 
            catch
            {
                return false;
            }
        }
    }
}
