using Microsoft.EntityFrameworkCore;

namespace FileForwaderCore.Database.Entities
{
    public class FileEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public string Link { get; set; }

        public string MessageId { get; set; }

        public MessageEntity Message { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<FileEntity>();

            entity.Property(_ => _.Id).ValueGeneratedOnAdd();

            entity.HasOne(_ => _.Message)
                .WithOne(_ => _.File)
                .HasForeignKey<FileEntity>(_ => _.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
