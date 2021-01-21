using AutoMapper;
using DutyCycle.API.Models;

namespace DutyCycle.API.Mapping
{
    public class ModelsMappingProfile : Profile
    {
        public ModelsMappingProfile()
        {
            CreateMap<CreateGroupRequest, GroupSettings>();
            CreateMap<GroupMemberInfo, Models.GroupMember>();

            CreateMap<Models.GroupActionTrigger, Triggers.GroupActionTrigger>().IncludeAllDerived();
            CreateMap<Models.SendSlackMessageTrigger, Triggers.SendSlackMessageTrigger>();

            CreateMap<Triggers.GroupActionTrigger, Models.GroupActionTrigger>().IncludeAllDerived();
            CreateMap<Triggers.SendSlackMessageTrigger, Models.SendSlackMessageTrigger>();
            
            CreateMap<GroupInfo, Models.Group>();
        }
    }
}