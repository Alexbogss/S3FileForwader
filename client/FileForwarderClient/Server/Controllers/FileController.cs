using FileForwarderClient.Server.Controllers.Payload;
using FileForwarderClient.Server.Controllers.Scheme;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refit;
using System.Threading.Tasks;

namespace FileForwarderClient.Server.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _host;

        private readonly ILogger<FileController> _logger;

        public FileController(
            Microsoft.Extensions.Configuration.IConfiguration config,
            ILogger<FileController> logger)
        {
            _host = config["CoreIpAddress"];
            _logger = logger;
        }

        [HttpPost("~/api/files/upload")]
        [DisableRequestSizeLimit]
        public async Task<string> UploadFile([FromForm]UploadPayload payload)
        {
            var info = payload.Info;
            var stream = payload.File.OpenReadStream();
            var streamPart = new StreamPart(stream, payload.File.FileName, payload.File.ContentType);
            var api = RestService.For<IFileApi>(_host);
            return await api.UploadFile(streamPart, info.CurrentChunk, info.TotalChunckCount);
        }
    }
}
