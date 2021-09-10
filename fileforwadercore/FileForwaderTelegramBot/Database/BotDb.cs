using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FileForwaderTelegramBot.Database
{
    public class BotDbFactory : IDesignTimeDbContextFactory<BotDb>
    {
        public BotDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BotDb>();
            optionsBuilder.UseSqlite("Data Source = testDb.db;");
            return new BotDb(optionsBuilder.Options);
        }
    }

    public class BotDb : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<ChatEntity> Chats { get; set; }

        public BotDb(DbContextOptions<BotDb> options) : base (options) { }

        public BotDb() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ChatEntity.Setup(modelBuilder);
        }
    }
}
