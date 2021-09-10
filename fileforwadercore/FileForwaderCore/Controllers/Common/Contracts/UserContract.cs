using FileForwaderCore.Database.Entities.Enums;

namespace FileForwaderCore.Controllers.Common.Contracts
{
    public class UserContract : BaseEntityContract
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
