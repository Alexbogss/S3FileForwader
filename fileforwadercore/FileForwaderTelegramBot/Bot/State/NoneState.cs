using FileForwaderTelegramBot.Database.Enums;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace FileForwaderTelegramBot.Bot.State
{
    public class NoneState : BaseChatState
    {
        public NoneState(ChatContext context) : base(context)
        { }

        public override async Task Handle()
        {
            _chatContext.Chat.State = ChatState.TokenRequest;
            await _chatContext.Db.SaveChangesAsync();

            await _chatContext.BotClient.SendTextMessageAsync(
                chatId: _chatContext.ChatId, 
                text: "Введите токен",
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
