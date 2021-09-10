using FileForwaderCore.Database.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileForwaderCore.Database.Entities
{
    public class StatusEventEntity : BaseEntity
    {
        public MessageStatus Value { get; set; }

        public DateTime EventDateTime { get; set; }

        public string MessageId { get; set; }
        
        public MessageEntity Message { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<StatusEventEntity>();

            entity.Property(_ => _.Id).ValueGeneratedOnAdd();

            entity.HasIndex(_ => _.Value);
            entity.HasIndex(_ => _.EventDateTime);

            entity.HasOne(_ => _.Message)
                .WithMany(_ => _.StatusEvents)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
