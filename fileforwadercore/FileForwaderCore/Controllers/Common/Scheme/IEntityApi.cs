using FileForwaderCore.Controllers.Common.Contracts;
using Refit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FileForwaderCore.Controllers.Common.Scheme
{
    public interface IEntityApi<T> where T : BaseEntityContract
    {
        [Post("")]
        Task<string> CreateEntity([Body] T contract);

        [Get("")]
        Task<T[]> GetAllEntities();

        [Get("/{id}")]
        Task<T> GetEntity([FromRoute] string id);

        [Put("")]
        Task UpdateEntity([Body] T contract);

        [Delete("/{id}")]
        Task DeleteEntity([FromRoute] string id);
    }
}
