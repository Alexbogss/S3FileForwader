using System.Threading.Tasks;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace FileForwaderCore.FileStorage
{
    public interface IFileStorageService
    {
        Task<ListBucketsResponse> GetBucketsList();

        Task<ListObjectsResponse> GetFilesList();

        Task<string> UploadFile(IFormFile file, int currentChunk, int totalChunck);
    }
}