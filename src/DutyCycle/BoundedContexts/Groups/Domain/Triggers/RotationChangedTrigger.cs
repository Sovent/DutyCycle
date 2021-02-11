using System;
using System.Threading.Tasks;

namespace DutyCycle.Groups.Domain.Triggers
{
    public abstract class RotationChangedTrigger
    {
        protected RotationChangedTrigger(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
        
        public abstract Task Run(GroupInfo group, TriggersContext triggersContext);
    }
}