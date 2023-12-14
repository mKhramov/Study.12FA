using FluentAssertions;
using Study.TFA.Domain.UseCases.GetTopics;

namespace Study.TFA.Domain.Tests.GetTopics
{
    public class GetTopicsQueryValidatorShould
    {
        private readonly GetTopicsQueryValidator sut = new();
        
        public static IEnumerable<object[]> GetInvalidQueries()
        {
            var validQuery = new GetTopicsQuery(Guid.Parse("B746D0ED-0C1C-4741-9785-558870902CDA"), 10, 5);

            yield return new object[] { validQuery with { ForumId = Guid.Empty } };
            yield return new object[] { validQuery with { Skip = -40 } };
            yield return new object[] { validQuery with { Take = -1 } };
        }

        [Fact]
        public void ReturnSuccess_WhenCommandIsValid()
        {
            var query = new GetTopicsQuery(
                Guid.Parse("B746D0ED-0C1C-4741-9785-558870902CDA"),
                10,
                5);
            sut.Validate(query).IsValid.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetInvalidQueries))]
        public void ReturnFailure_WhenQueryIsInvalid(GetTopicsQuery query)
        {
            sut.Validate(query).IsValid.Should().BeFalse();
        }
    }
}
