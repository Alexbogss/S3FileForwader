using FileForwaderCore.Controllers.Common.Contracts;
using Microsoft.AspNetCore.Http;
using Refit;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers.Common.Scheme
{
    public interface IFileApi
    {
        [Get("/api/files/message/{messageId}")]
        public Task<FileContract> GetMessageFile([AliasAs("messageId")] string messageId);

        [Multipart]
        [Post("/api/files/upload")]
        public Task<string> UploadFile(IFormFile file, int currentChunck, int totalChuncks);
    }
}
