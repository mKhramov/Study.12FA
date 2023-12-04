using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.GetForums
{
    public interface IGetForumsStorage
    {
        Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken);
    }
}
