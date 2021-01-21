using System;
using System.Threading.Tasks;

namespace DutyCycle.Triggers
{
    public abstract class GroupActionTrigger
    {
        protected GroupActionTrigger(Guid id, GroupAction action)
        {
            Id = id;
            Action = action;
        }

        public Guid Id { get; private set; }
        
        public GroupAction Action { get; private set; }
        
        public abstract Task Run(GroupInfo group, TriggersContext triggersContext);
    }
}