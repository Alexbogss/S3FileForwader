using AutoMapper;
using FileForwaderCore.Controllers.Common.Contracts;
using FileForwaderCore.Controllers.Common.Scheme;
using FileForwaderCore.Database.Entities;
using FileForwaderCore.Database.Repositories;
using FileForwaderCore.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers
{
    [Route("api/files")]
    public class FileController : BaseEntityController<FileContract, FileEntity>, IFileApi
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileStorageService _fileStorageService;

        public FileController(IRepository<FileEntity> repository, IMapper _mapper, IFileStorageService fileStorageService)
            : base(repository, _mapper)
        {
            _fileRepository = repository as IFileRepository;
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Получить контракт файла по идентификатору сообщения
        /// </summary>
        /// <param name="messageId"></param
        [HttpGet("message/{messageId}")]
        public async Task<FileContract> GetMessageFile(string messageId)
        {
            var result = await _fileRepository.GetMessageFile(messageId);

            return _mapper.Map<FileContract>(result);
        }

        /// <summary>
        /// Прикрепить к сообщению файл
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="file"></param>
        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<string> UploadFile(IFormFile file, int currentChunck, int totalChuncks)
        {
            return await _fileStorageService.UploadFile(file, currentChunck, totalChuncks);
        }
    }
}
