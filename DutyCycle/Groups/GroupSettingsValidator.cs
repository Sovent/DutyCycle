using System;
using DutyCycle.Errors;

namespace DutyCycle
{
    public class GroupSettingsValidator : IGroupSettingsValidator
    {
        private readonly ICronValidator _cronValidator;

        public GroupSettingsValidator(ICronValidator cronValidator)
        {
            _cronValidator = cronValidator ?? throw new ArgumentNullException(nameof(cronValidator));
        }
        
        public void Validate(GroupSettings groupSettings)
        {
            if (groupSettings == null) throw new ArgumentNullException(nameof(groupSettings));
            
            var now = DateTimeOffset.Now;
            if (string.IsNullOrEmpty(groupSettings.Name))
            {
                throw new InvalidGroupSettings("Group name must not be empty", now).ToException();
            }
            
            if (_cronValidator.IsValidCron(groupSettings.CyclingCronExpression))
            {
                throw new InvalidGroupSettings("Cron expression for group is in incorrect format", now).ToException();
            }

            if (groupSettings.DutiesCount < 1)
            {
                throw new InvalidGroupSettings("Group must have at least 1 duty", now).ToException();
            }
        }
    }
}