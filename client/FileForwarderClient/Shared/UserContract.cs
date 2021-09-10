using FileForwarderClient.Shared.Enums;

namespace FileForwarderClient.Shared
{
    public class UserContract : BaseEntityContract
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
