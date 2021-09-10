using AutoMapper;
using FileForwaderCore.Controllers.Common.Contracts;
using FileForwaderCore.Controllers.Common.Scheme;
using FileForwaderCore.Database;
using FileForwaderCore.Database.Entities;
using FileForwaderCore.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileForwaderCore.Controllers
{
    [Route("api/users")]
    public class UserController : BaseEntityController<UserContract, UserEntity>, IUserApi
    {
        private readonly FileForwaderDb _db;

        public UserController(IRepository<UserEntity> repository, IMapper mapper, FileForwaderDb db)
            : base(repository, mapper) 
        {
            _db = db;
        }

        [HttpGet("exist/{userId}")]
        public async Task EnsureUserExists(string userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(_ => _.Id == userId);
            if (user == null)
            {
                await _db.Users.AddAsync(new UserEntity
                {
                    Id = userId,
                    Name = Guid.NewGuid().ToString()
                });
            }

            await _db.SaveChangesAsync();
        }
    }
}
