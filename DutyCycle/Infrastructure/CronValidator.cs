using System;
using Cronos;

namespace DutyCycle.Infrastructure
{
    public class CronValidator : ICronValidator
    {
        public bool IsValidCron(string cron)
        {
            try
            {
                CronExpression.Parse(cron);
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}