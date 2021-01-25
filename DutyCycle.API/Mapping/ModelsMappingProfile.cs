using AutoMapper;
using Cronos;
using DutyCycle.API.Models;
using DutyCycle.Common;

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

            CreateMap<GroupInfo, Models.Group>()
                .ForMember(
                    model => model.CyclingCronExpression,
                    configuration => configuration.MapFrom(
                        groupInfo => groupInfo.CyclingCronExpression.ToString(CronFormat.Standard)));
        }
    }
}