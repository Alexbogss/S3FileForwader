using System.Collections.Generic;

namespace FileForwaderTelegramBot.Database
{
    public class UserEntity
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public ICollection<ChatEntity> Chats { get; set; }
    }
}
