using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicStorage
    {
        Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken);
        Task<Topic> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
    }
}
