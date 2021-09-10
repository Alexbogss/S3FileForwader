using System.Threading.Tasks;

namespace FileForwaderTelegramBot.Bot.State
{
    public abstract class BaseChatState
    {
        protected readonly ChatContext _chatContext;

        public BaseChatState(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public abstract Task Handle();
    }
}
