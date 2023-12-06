using Study.TFA.Domain.Authentication;

namespace Study.TFA.Domain.Authorization
{
    internal class IntentionManager : IIntentionManager
    {
        private readonly IEnumerable<IIntentionResolver> resolvers;
        private readonly IIdentityProvider identityProvider;

        public IntentionManager(
            IEnumerable<IIntentionResolver> resolvers,
            IIdentityProvider identityProvider)
        {
            this.resolvers = resolvers;
            this.identityProvider = identityProvider;
        }

        public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
        {
            var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
            return matchingResolver?.IsAllowed(identityProvider.Current, intention) ?? false;
        }

        public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct
        {
            throw new NotImplementedException();
        }
    }
}
