using FileForwaderCore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FileForwaderCore.Database.Repositories
{
    public interface IStatusEventRepository
    {
        IAsyncEnumerable<StatusEventEntity> GetMessageStatusHistory(string messageId);
    }

    public class StatusEventRepository : BaseRepository<StatusEventEntity>, IStatusEventRepository
    {
        public StatusEventRepository(FileForwaderDb db) : base (db)
        { }

        public IAsyncEnumerable<StatusEventEntity> GetMessageStatusHistory(string messageId) =>
            _db.StatusHistory
                .AsNoTracking()
                .Where(_ => _.MessageId == messageId)
                .AsAsyncEnumerable();
    }
}
