using AutoMapper;
using FileForwaderCore.Controllers.Common;
using FileForwaderCore.Controllers.Common.Contracts;
using FileForwaderCore.Controllers.Common.Scheme;
using FileForwaderCore.Database.Entities;
using FileForwaderCore.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers
{
    [Route("api/history")]
    public class StatusEventController : BaseEntityController<StatusEventContract, StatusEventEntity>, 
        IStatusEventApi
    {
        private readonly IStatusEventRepository _statusEventRepository;

        public StatusEventController(IRepository<StatusEventEntity> repository, IMapper mapper)
            : base(repository, mapper)
        {
            _statusEventRepository = repository as IStatusEventRepository;
        }

        /// <summary>
        /// История статусов сообщения по идентификатору сообщения
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet("message/{messageId}")]
        public async Task<StatusEventContract[]> GetHistory(string messageId)
        {
            var result = _statusEventRepository.GetMessageStatusHistory(messageId);

            return await result.ToContract<StatusEventContract, StatusEventEntity>(_mapper);
        }
    }
}
