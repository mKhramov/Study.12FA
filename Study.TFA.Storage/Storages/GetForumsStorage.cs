using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Study.TFA.Domain.UseCases.GetForums;

namespace Study.TFA.Storage.Storages
{
    internal class GetForumsStorage : IGetForumsStorage
    {
        private readonly IMemoryCache memoryCache;
        private readonly ForumDbContext dbContext;

        public GetForumsStorage(
            IMemoryCache memoryCache,
            ForumDbContext dbContext)
        {
            this.memoryCache = memoryCache;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken) => 
            await memoryCache.GetOrCreateAsync(
                nameof(GetForums),
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                    return dbContext.Forums
                        .Select(x => new Domain.Models.Forum
                        {
                            Id = x.ForumId,
                            Title = x.Title
                        })
                        .ToArrayAsync(cancellationToken);
                });
    }
}
