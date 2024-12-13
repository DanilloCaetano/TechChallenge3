using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MessagingService
{
    public interface IRabbitMqService
    {
        Task<bool> SendMessage<T>(string queueName, string routingKey, T bodyMessage);
        Task<IConnection> GetConnection(string hostName, string user, string pass);
    }
}
