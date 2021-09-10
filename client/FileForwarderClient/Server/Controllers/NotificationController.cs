using FileForwarderClient.Server.Controllers.Helpers;
using FileForwarderClient.Server.Controllers.Scheme;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refit;
using System.Threading.Tasks;

namespace FileForwarderClient.Server.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    public class NotificationController
    {
        private readonly string _host;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            Microsoft.Extensions.Configuration.IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            ILogger<NotificationController> logger)
        {
            _host = config["CoreIpAddress"];
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("~/api/notification/token")]
        public async Task<string> GetUserNotificationToken()
        {
            var userId = _httpContextAccessor.GetUserId();
            var api = RestService.For<INotificationApi>(_host);
            return await api.GetUserNotificationToken(userId);
        }
    }
}
