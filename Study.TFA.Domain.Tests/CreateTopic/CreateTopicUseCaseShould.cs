using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Study.TFA.Domain.Authentication;
using Study.TFA.Domain.Authorization;
using Study.TFA.Domain.Exceptions;
using Study.TFA.Domain.Models;
using Study.TFA.Domain.UseCases.CreateTopic;
using Study.TFA.Domain.UseCases.GetForums;

namespace Study.TFA.Domain.Tests.CreateTopic
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase sut;
        private readonly Mock<ICreateTopicStorage> storage;
        private readonly ISetup<ICreateTopicStorage, Task<Topic>> createTopicSetup;
        private readonly Mock<IGetForumsStorage> getForumsStorage;
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> getForumsSetup;
        private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetup;
        private readonly Mock<IIntentionManager> intentionManager;
        private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;

        public CreateTopicUseCaseShould()
        {
            storage = new Mock<ICreateTopicStorage>();            
            createTopicSetup = storage.Setup(s => s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

            getForumsStorage = new Mock<IGetForumsStorage>();
            getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

            var identity = new Mock<IIdentity>();
            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider.Setup(s => s.Current).Returns(identity.Object);
            getCurrentUserIdSetup = identity.Setup(s => s.UserId);

            intentionManager = new Mock<IIntentionManager>();
            intentionIsAllowedSetup = intentionManager.Setup(s => s.IsAllowed(It.IsAny<TopicIntention>()));

            var validator = new Mock<IValidator<CreateTopicCommand>>();
            validator.Setup(x => x.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            sut = new (validator.Object, intentionManager.Object, identityProvider.Object, getForumsStorage.Object, storage.Object);
        }

        [Fact]
        public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
        {
            var forumId = Guid.Parse("A1773E74-216F-421E-A8A2-1A752F127056");

            intentionIsAllowedSetup.Returns(false);

            await sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Whatever"), CancellationToken.None))
                .Should().ThrowAsync<IntentionManagerException>();
            intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {
            var forumId = Guid.Parse("104648EB-2A42-4D6B-98AD-DC293F73500D");

            intentionIsAllowedSetup.Returns(true);
            getForumsSetup.ReturnsAsync(Array.Empty<Forum>());

            await sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Some Title"), CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();

            getForumsStorage.Verify(s => s.GetForums(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
        {
            var forumId = Guid.Parse("673DDB47-716E-411F-8AAE-90F89A4A0B73");
            var userId = Guid.Parse("9223E3DB-DE7E-45D1-878D-6BD67DF6D443");

            intentionIsAllowedSetup.Returns(true);
            getForumsSetup.ReturnsAsync(new Forum[] { new Forum { Id = forumId } });
            getCurrentUserIdSetup.Returns(userId);
            var expected = new Topic();
            createTopicSetup.ReturnsAsync(expected);

            var actual = await sut.Execute(new CreateTopicCommand(forumId, "Hello World"), CancellationToken.None);
            actual.Should().Be(expected);

            storage.Verify(s => s.CreateTopic(forumId, userId, "Hello World", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}