using AutoMapper;
using FileForwaderCore.Controllers.Common;
using FileForwaderCore.Controllers.Common.Contracts;
using FileForwaderCore.Controllers.Common.Scheme;
using FileForwaderCore.Database.Entities;
using FileForwaderCore.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers
{
    public abstract class BaseEntityController<T, E> : ControllerBase, 
        IEntityApi<T> where T : BaseEntityContract
        where E : BaseEntity
    {
        protected readonly IRepository<E> _repository;
        protected readonly IMapper _mapper;

        public BaseEntityController(IRepository<E> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public Task<string> CreateEntity([FromBody] T contract)
        {
            return _repository.Insert(_mapper.Map<E>(contract));
        }

        [HttpDelete("{id}")]
        public async Task DeleteEntity([FromRoute] string id)
        {
            await _repository.DeleteById(id);
        }

        [HttpGet]
        public async Task<T[]> GetAllEntities()
        {
            return await _repository.GetAll().ToContract<T,E>(_mapper);
        }

        [HttpGet("{id}")]
        public async Task<T> GetEntity([FromRoute] string id)
        {
            var entity = await _repository.Get(id);

            return _mapper.Map<T>(entity);
        }

        [HttpPut]
        public async Task UpdateEntity([FromBody] T contract)
        {
            await _repository.Update(_mapper.Map<E>(contract));
        }
    }
}
