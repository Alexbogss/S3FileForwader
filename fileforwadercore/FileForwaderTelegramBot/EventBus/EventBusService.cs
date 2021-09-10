using FileForwaderTelegramBot.Bot;
using FileForwaderTelegramBot.Database;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileForwaderTelegramBot.EventBus
{
    public class EventBusService : IDisposable
    {
        private readonly BotClientHost _bot;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IConnection _connection;
        private readonly IModel _model;

        private const string RoutingKey = "notification_bus";

        private static readonly ILogger Log = Serilog.Log.ForContext<EventBusService>();

        public EventBusService(BotClientHost bot, IServiceScopeFactory scopeFactory)
        {
            _bot = bot;

            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _connection = connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.QueueDeclare(queue: RoutingKey, exclusive: false);

            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += ReceiveHandler;
            _model.BasicConsume(queue: RoutingKey,
                                 autoAck: true,
                                 consumer: consumer);
            _scopeFactory = scopeFactory;
        }

        private void ReceiveHandler(object o, BasicDeliverEventArgs ea)
        {
            var msg = Encoding.UTF8.GetString(ea.Body.ToArray());
            Log.Information("RabbitMQ: received {msg}", msg);

            var parameters = msg.Split(';');
            var messageType = parameters[0];
            var userId = parameters[1];
            var body = parameters[2];

            switch (messageType)
            {
                case "token":
                    HandleTokenNotification(userId, body);
                    break;

                case "message":
                    HangleNewMessage(userId, JsonConvert.DeserializeObject(body));
                    break;
            }
        }

        private void HandleTokenNotification(string userId, string token)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BotDb>();

            var user = db.Users.FirstOrDefault(_ => _.Id == userId);
            if (user == null)
            {
                user = new UserEntity
                {
                    Id = userId
                };
                db.Add(user);
            }

            user.Token = token;

            db.SaveChanges();
        }

        private void HangleNewMessage(string userId, dynamic message)
        {
            _bot.NotifyUser(userId,
                $"Новое сообщение \n" +
                $"от {message.SenderUserId} \n" +
                $"Описание: {message.Description} \n\n" +
                $"Файл\n" +
                $"Наименование: {message.FileName}\n" +
                $"Размер: {message.FileSize}\n" +
                $"Ссылка на скачивание: {message.FileLink}")
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public void Dispose()
        {
            _connection.Dispose();
            _model.Dispose();
        }
    }
}
