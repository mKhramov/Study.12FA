using Microsoft.EntityFrameworkCore;
using Study.TFA.Domain.UseCases.GetForums;

namespace Study.TFA.Storage.Storages
{
    public class GetForumsStorage : IGetForumsStorage
    {
        private readonly ForumDbContext dbContext;

        public GetForumsStorage(
            ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken) =>
            await dbContext.Forums
            .Select(x => new Domain.Models.Forum
            {
                Id = x.ForumId,
                Title = x.Title
            })
            .ToArrayAsync(cancellationToken);
    } 
}
