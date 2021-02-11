using AutoMapper;
using Cronos;
using DutyCycle.API.Models;
using DutyCycle.Common;
using DutyCycle.Groups.Domain;
using DutyCycle.Users;
using GroupSettings = DutyCycle.Groups.Domain.GroupSettings;
using RotationChangedTrigger = DutyCycle.Groups.Domain.Triggers.RotationChangedTrigger;
using SendSlackMessageTrigger = DutyCycle.Groups.Domain.Triggers.SendSlackMessageTrigger;
using UserCredentials = DutyCycle.Users.Domain.UserCredentials;

namespace DutyCycle.API.Mapping
{
    public class ModelsMappingProfile : Profile
    {
        public ModelsMappingProfile()
        {
            CreateMap<Models.GroupSettings, GroupSettings>();
            CreateMap<GroupMemberInfo, Models.GroupMember>();

            CreateMap<Models.RotationChangedTrigger, RotationChangedTrigger>().IncludeAllDerived();
            CreateMap<Models.SendSlackMessageTrigger, SendSlackMessageTrigger>();

            CreateMap<RotationChangedTrigger, Models.RotationChangedTrigger>().IncludeAllDerived();
            CreateMap<SendSlackMessageTrigger, Models.SendSlackMessageTrigger>();

            CreateMap<GroupInfo, Models.Group>()
                .ForMember(
                    model => model.CyclingCronExpression,
                    configuration => configuration.MapFrom(
                        groupInfo => groupInfo.CyclingCronExpression.ToString(CronFormat.Standard)));

            CreateMap<NewOrganizationInfo, Groups.Domain.Organizations.NewOrganizationInfo>();

            CreateMap<Models.UserCredentials, UserCredentials>();
        }
    }
}