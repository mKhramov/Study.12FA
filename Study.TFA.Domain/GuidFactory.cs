namespace Study.TFA.Domain
{
    public class GuidFactory : IGuidFactory
    {
        public Guid Create() => Guid.NewGuid();        
    }
}
