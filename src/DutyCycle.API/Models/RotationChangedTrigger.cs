using System;
using DutyCycle.Infrastructure;
using DutyCycle.Infrastructure.Json;

namespace DutyCycle.API.Models
{
    public abstract class RotationChangedTrigger : ISerializedWithDiscriminator
    {
        public Guid Id { get; set; }
        public abstract string Discriminator { get; }
    }
}