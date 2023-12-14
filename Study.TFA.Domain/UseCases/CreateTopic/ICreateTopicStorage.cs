using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicStorage
    {
        Task<Topic> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
    }
}
