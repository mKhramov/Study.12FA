using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicUseCase
    {
        Task<Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken);
    }
}
