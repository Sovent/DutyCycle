using System;
using DutyCycle.Groups.Application;
using DutyCycle.Groups.Domain;
using Hangfire;

namespace DutyCycle.Infrastructure
{
    public class RotationScheduler : IRotationScheduler
    {
        public RotationScheduler(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
        }
        
        public void ScheduleOrRescheduleForAGroup(GroupInfo groupInfo)
        {
            _recurringJobManager.AddOrUpdate<IGroupService>(
                GetJobId(groupInfo),
                service => service.RotateDutiesInGroup(groupInfo.Id), 
                groupInfo.CyclingCronExpression.ToString());
        }

        private static string GetJobId(GroupInfo groupInfo)
        {
            return $"RotateDutiesInGroup-{groupInfo.Id}";
        }
        
        private readonly IRecurringJobManager _recurringJobManager;
    }
}