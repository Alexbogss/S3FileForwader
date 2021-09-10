using FileForwarderClient.Shared;
using Refit;
using System.Threading.Tasks;

namespace FileForwarderClient.Server.Controllers.Scheme
{
    public interface IMessageApi
    {
        [Get("/api/messages/incoming/{userId}")]
        Task<MessageContract[]> GetUserIncomingMessages([AliasAs("userId")] string userId);

        [Get("/api/messages/outgoing/{userId}")]
        Task<MessageContract[]> GetUserOutgoingMessages([AliasAs("userId")] string userId);

        [Post("/api/messages/attach/{messageId}")]
        Task AttachFile([AliasAs("messageId")] string messageId, FileContract contract);
    }
}
