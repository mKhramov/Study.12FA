using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Study.TFA.Domain.Authentication;
using Study.TFA.Domain.Authorization;
using Study.TFA.Domain.Exceptions;
using Study.TFA.Domain.Models;
using Study.TFA.Domain.UseCases.CreateTopic;

namespace Study.TFA.Domain.Tests
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase sut;        
        private readonly Mock<ICreateTopicStorage> storage;
        private readonly ISetup<ICreateTopicStorage, Task<bool>> forumExistsSetup;
        private readonly ISetup<ICreateTopicStorage, Task<Topic>> createTopicSetup;
        private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetup;
        private readonly Mock<IIntentionManager> intentionManager;
        private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;

        public CreateTopicUseCaseShould()
        {
            storage = new Mock<ICreateTopicStorage>();
            forumExistsSetup = storage.Setup(s => s.ForumExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            createTopicSetup = storage.Setup(s => s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

            var identity = new Mock<IIdentity>();
            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider.Setup(s => s.Current).Returns(identity.Object);
            getCurrentUserIdSetup = identity.Setup(s => s.UserId);

            intentionManager = new Mock<IIntentionManager>();
            intentionIsAllowedSetup = intentionManager.Setup(s => s.IsAllowed(It.IsAny<TopicIntention>()));

            sut = new CreateTopicUseCase(intentionManager.Object, identityProvider.Object, storage.Object);
        }

        [Fact]
        public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
        {
            var forumId = Guid.Parse("A1773E74-216F-421E-A8A2-1A752F127056");

            intentionIsAllowedSetup.Returns(false);

            await sut.Invoking(s => s.Execute(forumId, "Whatever", CancellationToken.None))
                .Should().ThrowAsync<IntentionManagerException>();
            intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {
            var forumId = Guid.Parse("104648EB-2A42-4D6B-98AD-DC293F73500D");

            intentionIsAllowedSetup.Returns(true);
            forumExistsSetup.ReturnsAsync(false);

            await sut.Invoking(s => s.Execute(forumId, "Some Title", CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();

            storage.Verify(s => s.ForumExists(forumId, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
        {            
            var forumId = Guid.Parse("673DDB47-716E-411F-8AAE-90F89A4A0B73");
            var userId = Guid.Parse("9223E3DB-DE7E-45D1-878D-6BD67DF6D443");

            intentionIsAllowedSetup.Returns(true);
            forumExistsSetup.ReturnsAsync(true);
            getCurrentUserIdSetup.Returns(userId);
            var expected = new Topic();
            createTopicSetup.ReturnsAsync(expected);            
            
            var actual = await sut.Execute(forumId, "Hello World", CancellationToken.None);
            actual.Should().Be(expected);

            storage.Verify(s => s.CreateTopic(forumId, userId, "Hello World", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}