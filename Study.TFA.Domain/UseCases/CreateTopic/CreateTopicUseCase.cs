using Study.TFA.Domain.Exceptions;
using Study.TFA.Domain.Authentication;
using Study.TFA.Domain.Authorization;
using Study.TFA.Domain.Models;
using FluentValidation;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly IValidator<CreateTopicCommand> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly ICreateTopicStorage storage;

        public CreateTopicUseCase(
            IValidator<CreateTopicCommand> validator,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider, 
            ICreateTopicStorage storage)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.storage = storage;
        }

        public async Task<Models.Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var (forumId, title) = command;
            intentionManager.ThrowIfForbidden(TopicIntention.Create);

            var forumExists = await storage.ForumExists(forumId, cancellationToken);
            if (!forumExists)
            {
                throw new ForumNotFoundException(forumId);
            }

            return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
        }
    }
}
