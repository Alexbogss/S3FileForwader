using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileForwaderCore.FileStorage
{
    public class FileStorageService : IFileStorageService, IDisposable
    {
        private readonly AmazonS3Client _s3Client;
        
        private readonly string _directory;
        private readonly string _bucketName;

        public FileStorageService(IConfiguration config)
        {
            var localConfig = config.GetSection("Local");
            _directory = localConfig["Directory"];

            var s3Config = config.GetSection("S3");
            _bucketName = s3Config["Bucket"];
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = s3Config["Endpoint"]
            };
            _s3Client = new AmazonS3Client(s3Config["KeyId"], s3Config["SecretKey"], s3ClientConfig);
        }

        public async Task<ListBucketsResponse> GetBucketsList()
        {
            var buckets = await _s3Client.ListBucketsAsync();

            return buckets;
        }

        public async Task<ListObjectsResponse> GetFilesList()
        {
            var files = await _s3Client.ListObjectsAsync(new ListObjectsRequest()
            {
                BucketName = _bucketName,
            });

            return files;
        }

        public async Task<string> UploadFile(IFormFile file, int currentChunk, int totalChunck)
        {
            if (file == null)
                return null;

            string fullFilePath = Path.Combine(_directory, file.FileName);

            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            using (var fs = new FileStream(fullFilePath, FileMode.Append))
            {
                await file.CopyToAsync(fs);
            }

            if (currentChunk < totalChunck) 
                return null;

            Task.Run(async () => await UploadToS3(fullFilePath));

            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = file.FileName,
                Expires = DateTime.Now.AddDays(10)
            };
            
            return _s3Client.GetPreSignedURL(urlRequest);
        }

        private async Task UploadToS3(string fullFilePath)
        {
            using var fs = new FileStream(fullFilePath, FileMode.Open);
            var request = new TransferUtilityUploadRequest
            {
                InputStream = fs,
                Key = Path.GetFileName(fullFilePath),
                BucketName = _bucketName,
            };

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(request);

            File.Delete(fullFilePath);
        }

        public void Dispose()
        {
            _s3Client.Dispose();
        }
    }
}
