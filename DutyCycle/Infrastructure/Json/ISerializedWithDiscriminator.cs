namespace DutyCycle.Infrastructure
{
    public interface ISerializedWithDiscriminator
    {
        string Discriminator { get; }
    }
}