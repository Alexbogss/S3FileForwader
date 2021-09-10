using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FileForwaderTelegramBot.Database
{
    public static class DbHelpers
    {
        public static async Task<ChatEntity> GetChat(this BotDb db, long chatId) =>
            await db.Chats.FirstOrDefaultAsync(chat => chat.TelegramChatId == chatId);

        public static async Task<UserEntity> GetUserByToken(this BotDb db, string token) =>
            await db.Users.FirstOrDefaultAsync(user => user.Token == token);
    }
}
