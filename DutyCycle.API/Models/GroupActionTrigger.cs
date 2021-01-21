using System;
using DutyCycle.Triggers;

namespace DutyCycle.API.Models
{
    public abstract class GroupActionTrigger
    {
        public Guid Id { get; set; }
        public GroupAction Action { get; set; }
    }
}