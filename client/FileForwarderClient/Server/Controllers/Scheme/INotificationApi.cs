using Refit;
using System.Threading.Tasks;

namespace FileForwarderClient.Server.Controllers.Scheme
{
    public interface INotificationApi
    {
        [Get("/api/notification")]
        public Task<string> GetUserNotificationToken([AliasAs("userId")] string userId);
    }
}
