using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileForwarderClient.Shared;
using System.Threading.Tasks;
using Refit;
using FileForwarderClient.Server.Controllers.Scheme;
using FileForwarderClient.Server.Controllers.Helpers;

namespace FileForwarderClient.Server.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly string _host;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MessageController> _logger;

        public MessageController(
            Microsoft.Extensions.Configuration.IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            ILogger<MessageController> logger)
        {
            _host = config["CoreIpAddress"];
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("~/api/messages/incoming")]
        public async Task<MessageContract[]> GetIncoming()
        {
            var userId = _httpContextAccessor.GetUserId();
            var api = RestService.For<IMessageApi>(_host);
            return await api.GetUserIncomingMessages(userId);
        }

        [HttpGet("~/api/messages/outcoming")]
        public async Task<MessageContract[]> GetOutcoming()
        {
            var userId = _httpContextAccessor.GetUserId();
            var api = RestService.For<IMessageApi>(_host);
            return await api.GetUserOutgoingMessages(userId);
        }

        [HttpGet("~/api/message/{id}/events")]
        public async Task<StatusEventContract[]> GetStatuses(string id)
        {
            var api = RestService.For<IStatusEventApi>(_host);
            return await api.GetHistory(id);
        }

        [HttpPost("~/api/message/create")]
        public async Task<string> Create(MessageContract message)
        {
            var userId = _httpContextAccessor.GetUserId();
            message.SenderUserId = userId;
            var api = RestService.For<IEntityApi<MessageContract>>(_host + "/api/messages");
            return await api.CreateEntity(message);
        }

        [HttpGet("~/api/message/{id}/file")]
        public async Task<FileContract> GetFile(string id)
        {
            var api = RestService.For<IFileApi>(_host);
            return await api.GetMessageFile(id);
        }

        [HttpPost("~/api/message/attach/{id}")]
        public async Task<IActionResult> AttachFile(string id, FileContract contract)
        {
            var api = RestService.For<IMessageApi>(_host);
            await api.AttachFile(id, contract);
            return Ok();
        }
    }
}
