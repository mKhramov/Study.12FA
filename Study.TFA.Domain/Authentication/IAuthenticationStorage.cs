namespace Study.TFA.Domain.Authentication
{
    public interface IAuthenticationStorage
    {
        Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken);
    }
}
