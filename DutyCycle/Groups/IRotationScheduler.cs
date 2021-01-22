namespace DutyCycle
{
    public interface IRotationScheduler
    {
        void ScheduleOrRescheduleForAGroup(GroupInfo groupInfo);
    }
}