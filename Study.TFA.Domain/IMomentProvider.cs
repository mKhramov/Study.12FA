namespace Study.TFA.Domain
{
    public interface IMomentProvider
    {
        DateTimeOffset Now { get; }
    }
}
