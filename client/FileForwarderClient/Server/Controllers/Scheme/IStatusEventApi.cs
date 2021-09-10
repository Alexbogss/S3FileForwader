using FileForwarderClient.Shared;
using Refit;
using System.Threading.Tasks;

namespace FileForwarderClient.Server.Controllers.Scheme
{
    interface IStatusEventApi
    {
        [Get("/api/history/message/{messageId}")]
        public Task<StatusEventContract[]> GetHistory([AliasAs("messageId")] string messageId);
    }
}
