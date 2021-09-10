using FileForwaderCore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileForwaderCore.Database.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly FileForwaderDb _db;

        private DbSet<T> entities;

        public BaseRepository(FileForwaderDb db)
        {
            _db = db;

            entities = _db.Set<T>();
        }

        public IAsyncEnumerable<T> GetAll()
        {
            return entities
                .AsNoTracking()
                .AsAsyncEnumerable();
        }

        public async Task<T> Get(string id)
        {
            var value = await entities.SingleOrDefaultAsync(s => s.Id == id);

            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            return value;
        }

        public async Task<string> Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            entities.Add(entity);
            await _db.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            _db.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            entities.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(string id)
        {
            var entity = await entities.SingleOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            entities.Remove(entity);
            await _db.SaveChangesAsync();
        }

    }
}
