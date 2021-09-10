using AutoMapper;
using FileForwaderCore.Controllers.Common;
using FileForwaderCore.Controllers.Common.Contracts;
using FileForwaderCore.Controllers.Common.Scheme;
using FileForwaderCore.Database.Entities;
using FileForwaderCore.Database.Repositories;
using FileForwaderCore.MessageBroker;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers
{
    [Route("api/messages")]
    public class MessageController : BaseEntityController<MessageContract, MessageEntity>, IMessageApi
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageBrokerService _messageBroker;

        public MessageController(
            IRepository<MessageEntity> repository,
            IMapper mapper, IMessageBrokerService messageBroker) : base(repository, mapper)
        {
            _messageRepository = repository as IMessageRepository;
            _messageBroker = messageBroker;
        }

        /// <summary>
        /// Список входящий сообщений
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("incoming/{userId}")]
        public async Task<MessageContract[]> GetUserIncomingMessages(string userId)
        {
            var result = _messageRepository.GetUserIncomingMessages(userId);

            return await result.ToContract<MessageContract, MessageEntity>(_mapper);
        }

        /// <summary>
        /// Список входящий сообщений за сегодня
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("incoming/today/{userId}")]
        public async Task<MessageFileContract[]> GetUserTodayIncomingMessages(string userId)
        {
            var result = _messageRepository.GetUserTodayIncomingMessages(userId)
                .Select(Map);

            return await result.ToArrayAsync();
        }

        /// <summary>
        /// Список исходящих сообщений
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("outgoing/{userId}")]
        public async Task<MessageContract[]> GetUserOutgoingMessages(string userId)
        {
            var result = _messageRepository.GetUserOutgoingMessages(userId);

            return await result.ToContract<MessageContract, MessageEntity>(_mapper);
        }

        /// <summary>
        /// Присоеденить файл
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="contract"></param>
        [HttpPost("attach/{messageId}")]
        public async Task AttachFile(string messageId, [FromBody] FileContract contract)
        {
            var message = await _messageRepository.AttachFile(messageId, contract);
            _messageBroker.PublishNewMessage(Map(message));
        }

        private static MessageFileContract Map(MessageEntity message) =>
            new MessageFileContract
            {
                CreateDateTime = message.CreateDateTime,
                Description = message.Description,
                SenderUserId = message.SenderUserId,
                ReceiverUserId = message.ReceiverUserId,
                FileName = message.File?.Name,
                FileSize = message.File?.Size,
                FileLink = message.File?.Link
            };
    }
}
