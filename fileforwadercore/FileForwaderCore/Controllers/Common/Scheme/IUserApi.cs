using Refit;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers.Common.Scheme
{
    public interface IUserApi
    {
        [Get("/api/users/exist/{userId}")]
        Task EnsureUserExists(string userId);
    }
}
