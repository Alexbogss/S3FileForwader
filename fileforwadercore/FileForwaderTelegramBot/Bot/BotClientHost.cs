using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Serilog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using Microsoft.Extensions.DependencyInjection;
using FileForwaderTelegramBot.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FileForwaderTelegramBot.Bot
{
    public class BotClientHost
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;

        private static readonly ILogger Log = Serilog.Log.ForContext<BotClientHost>();

        public BotClientHost(IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _scopeFactory = scopeFactory;

            var token = _config["Token"];
            _botClient = new TelegramBotClient(token);
            Log.Information("Bot client host created: {Id}", _botClient.BotId);
        }

        public async Task Start(CancellationTokenSource cts)
        {
            _botClient.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cancellationToken: cts.Token);

            var me = await _botClient.GetMeAsync();

            Log.Information("Bot client host started {Name}", me.Username);
        }

        public async Task NotifyUser(string userId, string message)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BotDb>();

            await foreach(var chat in db.Chats
                .Where(c => c.UserId == userId)
                .AsAsyncEnumerable())
            {
                await _botClient.SendTextMessageAsync(chatId: chat.TelegramChatId, message);
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message),
                _ => UnknownUpdateHandler(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            Log.Information("Receive message type:{Type} text:{Text} from {ChatId}",
                message.Type, message.Text, message.Chat.Id);

            using var scope = _scopeFactory.CreateScope();
            var chatContext = scope.ServiceProvider.GetRequiredService<ChatContext>();
            await chatContext.ProcessMessage(_botClient, message);
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Log.Error("Message hande exception: {Text}", exception);

            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandler(Update update)
        {
            Log.Error("Unknown update type: {Type}", update.Type);

            return Task.CompletedTask;
        }
    }
}
