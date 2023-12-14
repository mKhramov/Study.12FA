using Study.TFA.Domain.Exceptions;

namespace Study.TFA.Domain.UseCases.GetForums
{
    internal static class GetForumsExtensions
    {
        private static async Task<bool> ForumExists(this IGetForumsStorage storage, 
            Guid forumId, 
            CancellationToken cancellationToken) 
        {
            var forums = await storage.GetForums(cancellationToken);
            return forums.Any(x => x.Id == forumId);
        }

        public static async Task ThrowIfForumNotFound(this IGetForumsStorage storage,
           Guid forumId,
           CancellationToken cancellationToken)
        {
            if (!await ForumExists(storage, forumId, cancellationToken))
            {
                throw new ForumNotFoundException(forumId);
            }
        }
    }
}
