using Study.TFA.Domain.Authentication;

namespace Study.TFA.Domain.Authorization
{
    public interface IIntentionResolver 
    {
    }

    public interface IIntentionResolver<in TIntention>: IIntentionResolver
    {
        bool IsAllowed(IIdentity subject, TIntention intention);
    }
}
