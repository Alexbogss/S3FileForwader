using Refit;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers.Common.Scheme
{
    public interface INotificationApi
    {
        [Get("/api/notification")]
        public Task<string> GetUserNotificationToken([AliasAs("userId")] string userId);
    }
}
