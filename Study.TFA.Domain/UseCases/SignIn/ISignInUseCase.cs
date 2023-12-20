using Study.TFA.Domain.Authentication;

namespace Study.TFA.Domain.UseCases.SignIn
{
    public interface ISignInUseCase
    {
        Task<(IIdentity identity, string token)> Execute(SignInCommand command, CancellationToken cancellationToken);
    }
}
