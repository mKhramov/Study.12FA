using Study.TFA.Domain.Authentication;
using Study.TFA.Domain.Authorization;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    public class TopicIntentionResolver : IIntentionResolver<TopicIntention>
    {
        public bool IsAllowed(IIdentity subject, TopicIntention intention) => intention switch
        {
            TopicIntention.Create => subject.IsAuthenticated(),
            _ => false
        };
    }
}
