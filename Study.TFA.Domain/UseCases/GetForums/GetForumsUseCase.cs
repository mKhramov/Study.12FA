using Study.TFA.Domain.Models;

namespace Study.TFA.Domain.UseCases.GetForums
{
    internal class GetForumsUseCase : IGetForumsUseCase
    {
        private readonly IGetForumsStorage storage;

        public GetForumsUseCase(
            IGetForumsStorage storage)
        {
            this.storage = storage;
        }

        public Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken) => 
            storage.GetForums(cancellationToken);
    }
}