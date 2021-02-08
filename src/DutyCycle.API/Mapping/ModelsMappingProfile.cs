using AutoMapper;
using Cronos;
using DutyCycle.API.Models;
using DutyCycle.Common;
using DutyCycle.Users;
using UserCredentials = DutyCycle.Users.UserCredentials;

namespace DutyCycle.API.Mapping
{
    public class ModelsMappingProfile : Profile
    {
        public ModelsMappingProfile()
        {
            CreateMap<Models.GroupSettings, GroupSettings>();
            CreateMap<GroupMemberInfo, Models.GroupMember>();

            CreateMap<Models.RotationChangedTrigger, Triggers.RotationChangedTrigger>().IncludeAllDerived();
            CreateMap<Models.SendSlackMessageTrigger, Triggers.SendSlackMessageTrigger>();

            CreateMap<Triggers.RotationChangedTrigger, Models.RotationChangedTrigger>().IncludeAllDerived();
            CreateMap<Triggers.SendSlackMessageTrigger, Models.SendSlackMessageTrigger>();

            CreateMap<GroupInfo, Models.Group>()
                .ForMember(
                    model => model.CyclingCronExpression,
                    configuration => configuration.MapFrom(
                        groupInfo => groupInfo.CyclingCronExpression.ToString(CronFormat.Standard)));

            CreateMap<NewOrganizationInfo, Organizations.NewOrganizationInfo>();

            CreateMap<Models.UserCredentials, UserCredentials>();
        }
    }
}