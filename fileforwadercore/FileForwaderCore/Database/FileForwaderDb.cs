using FileForwaderCore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileForwaderCore.Database
{
    public class FileForwaderDb : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<FileEntity> Files { get; set; }

        public DbSet<StatusEventEntity> StatusHistory { get; set; }

        public FileForwaderDb(DbContextOptions<FileForwaderDb> options) : base (options) { }

        public FileForwaderDb() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserEntity.Setup(modelBuilder);
            MessageEntity.Setup(modelBuilder);
            FileEntity.Setup(modelBuilder);
            StatusEventEntity.Setup(modelBuilder);
        }
    }
}
