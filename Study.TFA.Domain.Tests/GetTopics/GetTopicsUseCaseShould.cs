using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Study.TFA.Domain.Exceptions;
using Study.TFA.Domain.Models;
using Study.TFA.Domain.UseCases.GetForums;
using Study.TFA.Domain.UseCases.GetTopics;

namespace Study.TFA.Domain.Tests.GetTopics
{
    public class GetTopicsUseCaseShould
    {
        private readonly GetTopicsUseCase sut;
        private readonly Mock<IGetTopicsStorage> storage;
        private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> getTopicsSetup;        
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> getForumsSetup;

        public GetTopicsUseCaseShould()
        {
            storage = new Mock<IGetTopicsStorage>();
            getTopicsSetup = storage.Setup(s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

            var validator = new Mock<IValidator<GetTopicsQuery>>();
            validator.Setup(x => x.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            var getForumsStorage = new Mock<IGetForumsStorage>();
            getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

            sut = new GetTopicsUseCase(validator.Object, getForumsStorage.Object, storage.Object);
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoForum()
        {
            var forumId = Guid.Parse("B2773E74-216F-421E-A8A2-1A752F127057");
            getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = Guid.Parse("C3773E74-216F-421E-A8A2-1A752F127057") } });

            var query = new GetTopicsQuery(forumId, 5, 10);
            await sut.Invoking(x => x.Execute(query, CancellationToken.None))
                .Should()
                .ThrowExactlyAsync<ForumNotFoundException>();
        }

        [Fact]
        public async Task ReturnsTopics_ExtractedFromStorage_WhenForumExists()
        {
            var forumId = Guid.Parse("A1773E74-216F-421E-A8A2-1A752F127057");
            getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = forumId } });

            var expectedResources = new Topic[] { new() };
            var expectedTotalCount = 6;
            getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

            var (actualResources, actualTotalCount) = await sut.Execute(new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

            actualResources.Should().BeEquivalentTo(expectedResources);
            actualTotalCount.Should().Be(expectedTotalCount);
            storage.Verify(x => x.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
