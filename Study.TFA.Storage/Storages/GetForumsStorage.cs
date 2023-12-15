using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Study.TFA.Domain.UseCases.GetForums;

namespace Study.TFA.Storage.Storages
{
    internal class GetForumsStorage : IGetForumsStorage
    {
        private readonly IMemoryCache memoryCache;
        private readonly ForumDbContext dbContext;
        private readonly IMapper mapper;

        public GetForumsStorage(
            IMemoryCache memoryCache,
            ForumDbContext dbContext,
            IMapper mapper)
        {
            this.memoryCache = memoryCache;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken) => 
            await memoryCache.GetOrCreateAsync(
                nameof(GetForums),
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                    return dbContext.Forums
                        .ProjectTo<Domain.Models.Forum>(mapper.ConfigurationProvider)
                        .ToArrayAsync(cancellationToken);
                });
    }
}
