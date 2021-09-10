using FileForwaderTelegramBot.Database;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace FileForwaderTelegramBot.Bot.State
{
    public class TokenRequestState : BaseChatState
    {
        public TokenRequestState(ChatContext context) : base(context)
        { }

        public override async Task Handle()
        {
            var chatId = _chatContext.ChatId;
            var db = _chatContext.Db;
            var botClient = _chatContext.BotClient;

            var user = await db.GetUserByToken(_chatContext.Message.Text);
            if (user == null)
            {
                await botClient.SendTextMessageAsync(chatId: chatId, "Неверный токен, повторите попытку");

                return;
            }

            _chatContext.Chat.State = Database.Enums.ChatState.ReceivingMessages;
            _chatContext.Chat.UserId = user.Id;
            await _chatContext.Db.SaveChangesAsync();

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton { Text = "Остановить получение" }
                },
                new[]
                {
                    new KeyboardButton { Text = "Получить список всех сообщений за сегодня" }
                }
            });

            await _chatContext.BotClient.SendTextMessageAsync(
                chatId: _chatContext.ChatId,
                text: "Начато получение уведомлений о входящих файлах",
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
