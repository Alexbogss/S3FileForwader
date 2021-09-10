using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileForwarderClient.Shared;
using Refit;
using FileForwarderClient.Server.Controllers.Scheme;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using FileForwarderClient.Server.Controllers.Helpers;

namespace FileForwarderClient.Server.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _host;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IHttpContextAccessor httpContextAccessor,
            Microsoft.Extensions.Configuration.IConfiguration config,
            ILogger<UserController> logger)
        {
            _host = config["CoreIpAddress"];
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("~/api/users")]
        public async Task<UserContract[]> GetUsers()
        {
            var api = RestService.For<IEntityApi<UserContract>>(_host + "/api/users");
            return await api.GetAllEntities();
        }

        [HttpGet("~/api/users/exist")]
        public async Task<IActionResult> CheckUser()
        {
            var userId = _httpContextAccessor.GetUserId();
            var api = RestService.For<IUserApi>(_host);
            await api.EnsureUserExist(userId);
            return Ok();
        }
    }
}
