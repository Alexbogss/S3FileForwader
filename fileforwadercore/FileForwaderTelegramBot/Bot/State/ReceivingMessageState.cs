using FileForwaderTelegramBot.Controllers;
using FileForwaderTelegramBot.Database.Enums;
using Microsoft.EntityFrameworkCore;
using Refit;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace FileForwaderTelegramBot.Bot.State
{
    public class ReceivingMessageState : BaseChatState
    {
        public ReceivingMessageState(ChatContext context) : base(context) { }

        public override async Task Handle()
        {
            

            switch (_chatContext.Message.Text)
            {
                case "Остановить получение":
                    _chatContext.Chat.State = ChatState.None;
                    await _chatContext.Db.SaveChangesAsync();

                    await _chatContext.BotClient.SendTextMessageAsync(
                                    chatId: _chatContext.ChatId,
                                    text: "Для повторной подписки заново запросите токен и вызовите /start",
                                    replyMarkup: new ReplyKeyboardRemove());

                    break;

                case "Получить список всех сообщений за сегодня":
                    var api = RestService.For<IMessageApi>(_chatContext.Config["CoreUrl"]);

                    var user = await _chatContext.Db.Users
                        .FirstAsync(_ => _.Id == _chatContext.Chat.UserId);
                    var messages = await api.GetUserTodayIncomingMessages(user.Id);
                    foreach (var message in messages)
                    {
                        var text = $"Новое сообщение \n" +
                                        $"от {message.SenderUserId} \n" +
                                        $"Описание: {message.Description} \n\n" +
                                        $"Файл\n" +
                                        $"Наименование: {message.FileName}\n" +
                                        $"Размер: {message.FileSize}\n" +
                                        $"Ссылка на скачивание: {message.FileLink}";

                        await _chatContext.BotClient.SendTextMessageAsync(
                                    chatId: _chatContext.ChatId,
                                    text: text);
                    }
                    break;

                default:
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
                        text: "Выберите действие",
                        replyMarkup: replyKeyboardMarkup);
                    break;
            }
        }
    }
}
