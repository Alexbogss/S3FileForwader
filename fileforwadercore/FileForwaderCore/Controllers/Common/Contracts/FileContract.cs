namespace FileForwaderCore.Controllers.Common.Contracts
{
    public class FileContract : BaseEntityContract
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public string Link { get; set; }

        public string MessageId { get; set; }
    }
}
