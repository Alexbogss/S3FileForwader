namespace FileForwarderClient.Shared
{
    public class UploadFileInfo
    {
        public int CurrentChunk { get; set; }

        public int TotalChunckCount { get; set; }

        public int ChunckSize { get; set; }

        public long TotalSize { get; set; }
    }
}
