using FileForwarderClient.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileForwarderClient.Server.Controllers.Payload
{
    public class UploadPayload
    {
        public IFormFile File { get; set; }

        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public UploadFileInfo Info { get; set; }
    }
}
