using FileForwaderCore.Database.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FileForwaderCore.Database.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public ICollection<MessageEntity> SentMessages { get; set; }

        public ICollection<MessageEntity> ReceivedMessages { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<UserEntity>();

            entity.Property(_ => _.Id).ValueGeneratedOnAdd();
        }
    }
}
