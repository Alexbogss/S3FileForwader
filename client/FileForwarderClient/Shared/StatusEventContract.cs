using FileForwarderClient.Shared.Enums;
using System;

namespace FileForwarderClient.Shared
{
    public class StatusEventContract : BaseEntityContract
    {
        public MessageStatus Value { get; set; }

        public DateTime EventDateTime { get; set; }

        public string MessageId { get; set; }
    }
}
