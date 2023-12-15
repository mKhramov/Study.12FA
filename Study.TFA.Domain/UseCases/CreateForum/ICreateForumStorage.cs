using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.CreateForum
{
    public interface ICreateForumStorage
    {
        Task<Forum> Create(string title, CancellationToken cancellationToken);
    }
}
