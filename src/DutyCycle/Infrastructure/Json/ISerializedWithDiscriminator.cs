namespace DutyCycle.Infrastructure.Json
{
    public interface ISerializedWithDiscriminator
    {
        string Discriminator { get; }
    }
}