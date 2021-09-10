using Amazon.S3.Model;
using FileForwaderCore.FileStorage;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers
{
    [Route("/api/storage")]
    public class StorageController
    {
        private readonly IFileStorageService _fileStorageService;

        public StorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Список бакетов в S3 хранилище
        /// </summary>
        [HttpGet("buckets/list")]
        public async Task<ListBucketsResponse> GetBucketsList()
        {
            return await _fileStorageService.GetBucketsList();
        }

        [HttpGet("objects/list")]
        public async Task<ListObjectsResponse> GetFilesList()
        {
            return await _fileStorageService.GetFilesList();
        }
    }
}
