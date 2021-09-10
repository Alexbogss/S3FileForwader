using FileForwarderClient.Shared;
using Refit;
using System.Threading.Tasks;

namespace FileForwarderClient.Server.Controllers.Scheme
{
    public interface IFileApi
    {
        [Get("/api/files/message/{messageId}")]
        public Task<FileContract> GetMessageFile([AliasAs("messageId")] string messageId);

        [Multipart]
        [Post("/api/files/upload")]
        public Task<string> UploadFile(StreamPart file, int currentChunck, int totalChuncks);
    }
}
