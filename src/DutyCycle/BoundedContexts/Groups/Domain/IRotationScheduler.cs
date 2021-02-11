namespace DutyCycle.Groups.Domain
{
    public interface IRotationScheduler
    {
        void ScheduleOrRescheduleForAGroup(GroupInfo groupInfo);
    }
}