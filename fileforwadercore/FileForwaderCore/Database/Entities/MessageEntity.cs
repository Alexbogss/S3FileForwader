using FileForwaderCore.Database.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FileForwaderCore.Database.Entities
{
    public class MessageEntity : BaseEntity
    {
        public DateTime CreateDateTime { get; set; }

        public MessageStatus CurrentStatus { get; set; }

        public string Description { get; set; }

        public string Error { get; set; }

        public FileEntity File { get; set; }

        public string SenderUserId { get; set; }

        public UserEntity SenderUser { get; set; }

        public string ReceiverUserId { get; set; }

        public UserEntity ReceiverUser { get; set; }

        public ICollection<StatusEventEntity> StatusEvents { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<MessageEntity>();

            entity.Property(_ => _.Id).ValueGeneratedOnAdd();

            entity.HasIndex(_ => _.CreateDateTime);
            entity.HasIndex(_ => _.CurrentStatus);

            entity.HasOne(_ => _.SenderUser)
                .WithMany(_ => _.SentMessages)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(_ => _.ReceiverUser)
                .WithMany(_ => _.ReceivedMessages)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
