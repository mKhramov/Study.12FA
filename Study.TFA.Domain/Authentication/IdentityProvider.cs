namespace Study.TFA.Domain.Authentication
{
    internal class IdentityProvider : IIdentityProvider
    {
        public IIdentity Current => new User(Guid.Parse("FDD4FE8E-8758-4D26-9562-FB629344F3B5"));
    }
}
