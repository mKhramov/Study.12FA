using Study.TFA.Domain.Authentication;
using Study.TFA.Domain.Authorization;

namespace Study.TFA.Domain.UseCases.CreateForum
{
    internal class ForumIntentionResolver : IIntentionResolver<ForumIntention>
    {
        public bool IsAllowed(IIdentity subject, ForumIntention intention) => intention switch
        {
            ForumIntention.Create => subject.IsAuthenticated(),
            _ => false
        };
    }
}
