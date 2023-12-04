using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.GetForums
{
    public interface IGetForumsUseCase
    {
        Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken);
    }
}
