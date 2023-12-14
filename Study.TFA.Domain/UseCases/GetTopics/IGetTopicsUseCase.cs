using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.GetTopics
{
    public interface IGetTopicsUseCase
    {
        Task<(IEnumerable<Topic> resources, int totalCount)> Execute(GetTopicsQuery query,
            CancellationToken cancellationToken);
    }
}
