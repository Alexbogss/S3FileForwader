using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FileForwaderCore.Database
{
    public class FileForwaderDbFactory : IDesignTimeDbContextFactory<FileForwaderDb>
    {
        public FileForwaderDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FileForwaderDb>();

            // TODO from args (for test?)
            var connectionString = "Server=localhost;Port=5432;Database=hack;User Id=postgres;Password=1234;";

            optionsBuilder.UseNpgsql(connectionString);

            return new FileForwaderDb(optionsBuilder.Options);
        }
    }
}
