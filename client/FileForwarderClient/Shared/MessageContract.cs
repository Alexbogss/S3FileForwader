using System;

namespace FileForwarderClient.Shared
{
    public class MessageContract : BaseEntityContract
    {
        public DateTime CreateDateTime { get; set; }

        public string Description { get; set; }

        public string Error { get; set; }

        public string SenderUserId { get; set; }

        public string ReceiverUserId { get; set; }

        public string QueryId => Id;
    }
}
