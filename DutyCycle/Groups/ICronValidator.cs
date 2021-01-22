namespace DutyCycle
{
    public interface ICronValidator
    {
        bool IsValidCron(string cron);
    }
}