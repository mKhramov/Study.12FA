using Study.TFA.Domain.Authentication;

namespace Study.TFA.Domain.UseCases.SignOn
{
    public interface ISignOnUseCase
    {
        Task<IIdentity> Execute(SignOnCommand command, CancellationToken cancellationToken);
    }
}
