using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutyCycle.Triggers
{
    public class GroupActionTrigger
    {
        public GroupActionTrigger(GroupAction action)
        {
            Action = action;
        }
        
        public GroupAction Action { get; private set; }

        public async Task Run(Group group, TriggersTooling triggersTooling)
        {
            var callbackTasks = _callbacks.Select(callback => callback.Execute(group, triggersTooling));
            await Task.WhenAll(callbackTasks);
        }

        public void AddCallback(TriggerCallback callback)
        {
            _callbacks.Add(callback);
        }

        public bool TryRemoveCallback(Guid callbackToRemoveId)
        {
            var elementsRemoved = _callbacks.RemoveAll(callback => callback.Id == callbackToRemoveId);
            return elementsRemoved != 0;
        }

        private List<TriggerCallback> _callbacks = new List<TriggerCallback>();
    }
}