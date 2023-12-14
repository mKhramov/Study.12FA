using Study.TFA.Domain.Authentication;
using Study.TFA.Domain.Authorization;
using FluentValidation;
using Study.TFA.Domain.UseCases.GetForums;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly IValidator<CreateTopicCommand> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly IGetForumsStorage getForumsStorage;
        private readonly ICreateTopicStorage storage;

        public CreateTopicUseCase(
            IValidator<CreateTopicCommand> validator,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider, 
            IGetForumsStorage getForumsStorage,
            ICreateTopicStorage storage)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.getForumsStorage = getForumsStorage;
            this.storage = storage;
        }

        public async Task<Models.Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var (forumId, title) = command;
            intentionManager.ThrowIfForbidden(TopicIntention.Create);

            await getForumsStorage.ThrowIfForumNotFound(forumId, cancellationToken);

            return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
        }
    }
}
