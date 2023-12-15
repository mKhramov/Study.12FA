using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.CreateForum
{
    public interface ICreateForumUseCase
    {
        Task<Forum> Execute(CreateForumCommand command, CancellationToken cancellationToken);
    }
}
