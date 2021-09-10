using FileForwaderCore.Controllers.Common.Scheme;
using FileForwaderCore.MessageBroker;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers
{
    [Route("api/notification")]
    public class NotificationController : ControllerBase, INotificationApi
    {
        private readonly IMessageBrokerService _messageBroker;

        public NotificationController(IMessageBrokerService messageBroker)
        {
            _messageBroker = messageBroker;
        }

        [HttpGet]
        public Task<string> GetUserNotificationToken(string userId)
        {
            var token = Guid.NewGuid().ToString();
            _messageBroker.PublishNotifyToken(userId, token);

            return Task.FromResult(token);
        }
    }
}
