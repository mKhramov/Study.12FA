namespace Study.TFA.Domain.Authentication
{
    public class User : IIdentity
    {
        public User(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}