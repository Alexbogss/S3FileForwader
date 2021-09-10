using FileForwaderCore.Controllers.Common.Contracts;
using Refit;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers.Common.Scheme
{
    interface IStatusEventApi
    {
        [Get("api/history/message/{messageId}")]
        public Task<StatusEventContract[]> GetHistory([AliasAs("messageId")] string messageId);
    }
}
