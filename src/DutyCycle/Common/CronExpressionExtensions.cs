using Cronos;

namespace DutyCycle.Common
{
    public static class CronExpressionExtensions
    {
        public static string ToString(this CronExpression cronExpression, CronFormat cronFormat)
        {
            var cronExpressionString = cronExpression.ToString();
            if (cronFormat == CronFormat.Standard)
            {
                // todo: removes seconds from string representation
                return cronExpressionString.Substring(2);
            }

            return cronExpressionString;
        }
    }
}