using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers.Common
{
    public static class ApiHelpers
    {
        public static async Task<T[]> ToContract<T, E>(
            this IAsyncEnumerable<E> result, IMapper mapper) =>
            await result
                .Select(_ => mapper.Map<T>(_))
                .ToArrayAsync();
    }
}
