using FileForwaderTelegramBot.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace FileForwaderTelegramBot.Database
{
    public class ChatEntity
    {
        public long Id { get; set; }

        public long TelegramChatId { get; set; }

        public ChatState State { get; set; }

        public string UserId { get; set; }

        public UserEntity User { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ChatEntity>();

            entity.HasOne(_ => _.User)
                .WithMany(_ => _.Chats)
                .OnDelete(DeleteBehavior.Cascade);
        } 
    }
}
