using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Study.TFA.Domain.UseCases.SignIn;

namespace Study.TFA.Storage.Storages
{
    internal class SignInStorage : ISignInStorage
    {
        private readonly ForumDbContext dbContext;
        private readonly IMapper mapper;

        public SignInStorage(
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
