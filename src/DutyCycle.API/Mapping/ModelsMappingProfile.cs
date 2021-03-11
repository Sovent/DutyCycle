using AutoMapper;
using Cronos;
using DutyCycle.API.Models;
using DutyCycle.Common;
using DutyCycle.Groups.Domain;
using DutyCycle.Groups.Domain.Organizations;
using DutyCycle.Users;
using GroupSettings = DutyCycle.Groups.Domain.GroupSettings;
using NewOrganizationInfo = DutyCycle.API.Models.NewOrganizationInfo;
using OrganizationInfo = DutyCycle.Groups.Domain.Organizations.OrganizationInfo;
using OrganizationSlackInfo = DutyCycle.Groups.Domain.Organizations.OrganizationSlackInfo;
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

            CreateMap<OrganizationSlackInfo, Models.OrganizationSlackInfo>();
            CreateMap<OrganizationInfo, Models.OrganizationInfo>()
                .ForMember(
                    info => info.SlackInfo,
                    configuration =>
                        configuration.MapFrom(domainInfo => domainInfo.SlackInfo.IfNoneUnsafe(() => null)));

            CreateMap<Models.UserCredentials, UserCredentials>();
        }
    }
}