using AutoMapper;
using DutyCycle.API.Models;

namespace DutyCycle.API.Mapping
{
    public class ModelsMappingProfile : Profile
    {
        public ModelsMappingProfile()
        {
            CreateMap<CreateGroupRequest, GroupSettings>();
            CreateMap<GroupMember, Models.GroupMember>();
            CreateMap<Group, Models.Group>();
        }
    }
}