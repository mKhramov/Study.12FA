using FluentAssertions;
using Study.TFA.Domain.UseCases.CreateTopic;

namespace Study.TFA.Domain.Tests
{
    public class CreateTopicCommandValidatorShould
    {
        private readonly CreateTopicCommandValidator sut = new();

        [Fact]
        public void ReturnSuccess_WhenCommandIsValid()
        {
            var actual = sut.Validate(new CreateTopicCommand(Guid.Parse("B746D0ED-0C1C-4741-9785-558870902CDF"), "Hello"));
            actual.IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidCommandsWithCodes()
        {
            var validCommand = new CreateTopicCommand(Guid.Parse("5575C79D-8338-40C7-9286-0D371EB02409"), "Hello");

            yield return new object[] { validCommand with { ForumId = Guid.Empty }, nameof(CreateTopicCommand.ForumId), "Empty" };
            yield return new object[] { validCommand with { Title = string.Empty }, nameof(CreateTopicCommand.Title), "Empty" };
            yield return new object[] { validCommand with { Title = "   " }, nameof(CreateTopicCommand.Title), "Empty" };
            yield return new object[] { validCommand with { Title = string.Join("a", Enumerable.Range(0, 100)) }, nameof(CreateTopicCommand.Title), "TooLong" };
        }

        public static IEnumerable<object[]> GetInvalidCommands()
        {
            var validCommand = new CreateTopicCommand(Guid.Parse("5575C79D-8338-40C7-9286-0D371EB02409"), "Hello");

            yield return new object[] { validCommand with { ForumId = Guid.Empty }};
            yield return new object[] { validCommand with { Title = string.Empty }};
            yield return new object[] { validCommand with { Title = "   " } };
            yield return new object[] { validCommand with { Title = string.Join("a", Enumerable.Range(0, 100)) }};
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommands))]
        public void ReturnFailure_WhenCommandIsInvalid(CreateTopicCommand command)
        {
            var actual = sut.Validate(command);
            actual.IsValid.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommandsWithCodes))]
        public void ReturnFailure_WhenCommandIsInvalid_WithPropertyNameAndErrorCode(
            CreateTopicCommand command,
            string expectedInvalidPropertyName,
            string expectedErrorCode)
        {
            var actual = sut.Validate(command);
            actual.IsValid.Should().BeFalse();
            actual.Errors.Should()
                .Contain(x => x.PropertyName == expectedInvalidPropertyName && x.ErrorCode == expectedErrorCode);
        }

        
    }
}
