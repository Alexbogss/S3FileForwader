using FileForwaderTelegramBot.Bot.State;
using FileForwaderTelegramBot.Database;
using FileForwaderTelegramBot.Database.Enums;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FileForwaderTelegramBot.Bot
{
    public class ChatContext
    {
        public BotDb Db
        {
            get { return _db; }
        }
        private readonly BotDb _db;

        public Message Message
        {
            get { return _message; }
        }
        private Message _message;

        public ITelegramBotClient BotClient
        {
            get { return _botClient; }
        }
        private ITelegramBotClient _botClient;

        public ChatEntity Chat
        {
            get { return _chat; }
        }
        private ChatEntity _chat;

        public IConfiguration Config
        {
            get { return _config; }
        }
        private IConfiguration _config;

        public long ChatId => Message.Chat.Id;

        public ChatContext(BotDb db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task ProcessMessage(ITelegramBotClient botClient, Message message)
        {
            _botClient = botClient;
            _message = message;

            _chat = await Db.GetChat(ChatId);
            if (_chat == null)
            {
                _chat = new ChatEntity
                {
                    TelegramChatId = ChatId,
                    State = ChatState.None
                };

                await _db.Chats.AddAsync(_chat);
                await _db.SaveChangesAsync();
            }

            await ProcessUserState();
        }

        public async Task ProcessUserState()
        {
            var stateAction = _chat.State switch
            {
                ChatState.TokenRequest => new TokenRequestState(this).Handle(),
                ChatState.ReceivingMessages => new ReceivingMessageState(this).Handle(),
                _ => new NoneState(this).Handle(),
            };

            await stateAction;
        }
    }
}
