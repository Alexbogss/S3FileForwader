using Refit;
using System.Threading.Tasks;

namespace FileForwarderClient.Server.Controllers.Scheme
{
    public interface IUserApi
    {
        [Get("/api/users/exist/{userId}")]
        Task EnsureUserExist([AliasAs("userId")]string userId);
    }
}
