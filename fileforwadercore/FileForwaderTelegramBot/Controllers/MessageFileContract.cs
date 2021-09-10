using System;

namespace FileForwaderTelegramBot.Controllers
{
    public class MessageFileContract
    {
        public DateTime CreateDateTime { get; set; }

        public string Description { get; set; }

        public string SenderUserId { get; set; }

        public string ReceiverUserId { get; set; }

        public string FileName { get; set; }

        public string FileSize { get; set; }

        public string FileLink { get; set; }
    }
}
