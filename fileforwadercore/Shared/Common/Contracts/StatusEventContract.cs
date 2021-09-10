using FileForwaderCore.Database.Entities.Enums;
using System;

namespace FileForwaderCore.Controllers.Common.Contracts
{
    public class StatusEventContract : BaseEntityContract
    {
        public MessageStatus Value { get; set; }

        public DateTime EventDateTime { get; set; }

        public string MessageId { get; set; }
    }
}
