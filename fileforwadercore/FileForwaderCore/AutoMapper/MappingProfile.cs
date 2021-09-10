using AutoMapper;
using FileForwaderCore.Controllers.Common.Contracts;
using FileForwaderCore.Database.Entities;

namespace FileForwaderCore.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserContract, UserEntity>();
            CreateMap<MessageContract, MessageEntity>();
            CreateMap<FileContract, FileEntity>();
            CreateMap<StatusEventContract, StatusEventEntity>();

            CreateMap<UserEntity, UserContract>();
            CreateMap<MessageEntity, MessageContract>();
            CreateMap<FileEntity, FileContract>();
            CreateMap<StatusEventEntity, StatusEventContract>();
        }
    }
}
