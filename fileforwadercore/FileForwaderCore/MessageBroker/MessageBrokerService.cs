using FileForwaderCore.Controllers.Common.Contracts;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Text;
using System.Text.Json;

namespace FileForwaderCore.MessageBroker
{
    public interface IMessageBrokerService
    {
        void PublishNotifyToken(string userId, string token);

        void PublishNewMessage(MessageFileContract message);
    }

    public class MessageBrokerService : IMessageBrokerService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _model;

        private const string RoutingKey = "notification_bus";

        private readonly static ILogger Log = Serilog.Log.ForContext<IMessageBrokerService>();

        public MessageBrokerService()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _connection = connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.QueueDeclare(queue: RoutingKey, exclusive: false);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _model.Dispose();
        }

        public void PublishNotifyToken(string userId, string token)
        {
            var msg = $"token;{userId};{token}";

            PublishMessage(msg);
        }

        public void PublishNewMessage(MessageFileContract message)
        {
            var msg = $"message;{message.ReceiverUserId};{JsonSerializer.Serialize(message)}";

            PublishMessage(msg);
        }

        private void PublishMessage(string msg)
        {
            _model.BasicPublish(exchange: "",
                routingKey: RoutingKey,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(msg));

            Log.Information("RabbitMQ: sent {msg}", msg);
        }
    }
}
