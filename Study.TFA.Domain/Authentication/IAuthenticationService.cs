namespace Study.TFA.Domain.Authentication
{
    public interface IAuthenticationService
    {
        Task<(bool success, string authToken)> SingIn(BasicSignInCredentials credentials, CancellationToken cancellationToken);
        Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken);
    }
}
