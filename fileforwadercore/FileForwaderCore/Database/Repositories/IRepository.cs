using FileForwaderCore.Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileForwaderCore.Database.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IAsyncEnumerable<T> GetAll();
        Task<T> Get(string id);
        Task<string> Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task DeleteById(string id);
    }
}
