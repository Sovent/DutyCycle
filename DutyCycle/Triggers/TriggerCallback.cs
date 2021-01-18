using System;
using System.Threading.Tasks;

namespace DutyCycle.Triggers
{
    public abstract class TriggerCallback
    {
        protected TriggerCallback(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
        public abstract Task Execute(Group group, TriggersTooling triggersTooling);
    }
}