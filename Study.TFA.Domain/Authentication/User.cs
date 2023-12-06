namespace Study.TFA.Domain.Authentication
{
    internal class User : IIdentity
    {
        public User(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}