using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Study.TFA.Domain.Authentication;

namespace Study.TFA.Storage.Storages
{
    internal class AuthenticationStorage : IAuthenticationStorage
    {
        private readonly ForumDbContext dbContext;
        private readonly IMapper mapper;

        public AuthenticationStorage(
            ForumDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken) => dbContext.Users
            .Where(x => x.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
