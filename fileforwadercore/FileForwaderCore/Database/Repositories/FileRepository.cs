using FileForwaderCore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FileForwaderCore.Database.Repositories
{
    public interface IFileRepository
    {
        Task<FileEntity> GetMessageFile(string messageId);
    }

    public class FileRepository : BaseRepository<FileEntity>, IFileRepository
    {
        public FileRepository(FileForwaderDb db) : base(db) { }

        public async Task<FileEntity> GetMessageFile(string messageId) =>
            await _db.Files
                .AsNoTracking()
                .SingleAsync(_ => _.MessageId == messageId);
    }
}
