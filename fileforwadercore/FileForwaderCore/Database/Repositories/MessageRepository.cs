using FileForwaderCore.Controllers.Common.Contracts;
using FileForwaderCore.Database.Entities;
using FileForwaderCore.Database.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileForwaderCore.Database.Repositories
{
    public interface IMessageRepository
    {
        IAsyncEnumerable<MessageEntity> GetUserIncomingMessages(string userId);

        IAsyncEnumerable<MessageEntity> GetUserTodayIncomingMessages(string userId);

        IAsyncEnumerable<MessageEntity> GetUserOutgoingMessages(string userId);

        Task<MessageEntity> AttachFile(string messageId, FileContract contract);
    }

    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository
    {
        public MessageRepository(FileForwaderDb db) : base(db)
        { }

        public IAsyncEnumerable<MessageEntity> GetUserIncomingMessages(string userId) => 
            _db.Messages
                .AsNoTracking()
                .Where(_ => _.ReceiverUserId == userId)
                .AsAsyncEnumerable();

        public IAsyncEnumerable<MessageEntity> GetUserTodayIncomingMessages(string userId) =>
            _db.Messages
                .AsNoTracking()
                .Include(_ => _.File)
                .Where(_ => _.ReceiverUserId == userId /*&& _.CreateDateTime.Date == DateTime.Now.Date*/)
                .AsAsyncEnumerable();

        public IAsyncEnumerable<MessageEntity> GetUserOutgoingMessages(string userId) =>
            _db.Messages
                .AsNoTracking()
                .Where(_ => _.SenderUserId == userId)
                .AsAsyncEnumerable();

        public async Task<MessageEntity> AttachFile(string messageId, FileContract contract)
        {
            var message = await Get(messageId);
            message.StatusEvents ??= new List<StatusEventEntity>();

            message.StatusEvents.Add(new StatusEventEntity
            {
                EventDateTime = DateTime.Now,
                Value = MessageStatus.Uploaded,
            });

            message.File = new FileEntity
            {
                Name = contract.Name,
                Size = contract.Size,
                Link = contract.Link
            };

            await Update(message);

            return message;
        }
    }
}
