using Microsoft.EntityFrameworkCore;
using Study.TFA.Domain.UseCases.GetTopics;

namespace Study.TFA.Storage.Storages
{
    internal class GetTopicsStorage : IGetTopicsStorage
    {
        private readonly ForumDbContext dbContext;

        public GetTopicsStorage(
            ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<(IEnumerable<Domain.Models.Topic> resources, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken cancellationToken)
        {
            var query = dbContext.Topics.Where(x => x.ForumId == forumId);

            var totalCount = await query.CountAsync(cancellationToken);
            var resources = await query.OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(take)
                .Select(x => new Domain.Models.Topic
                {
                    CreatedAt = x.CreatedAt,
                    ForumId = x.ForumId,
                    Id = x.TopicId,
                    Title = x.Title,
                    UserId = x.UserId
                })
                .ToArrayAsync(cancellationToken);

            return (resources, totalCount);
        }
    }
}
