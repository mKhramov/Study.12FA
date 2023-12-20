using FluentAssertions;
using Study.TFA.Domain.Authentication;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Study.TFA.E2E
{
    public class AccountEndpointsShould: IClassFixture<ForumApiApplicationFactory>
    {
        private readonly ForumApiApplicationFactory factory;
        private readonly ITestOutputHelper testOutputHelper;

        public AccountEndpointsShould(
            ForumApiApplicationFactory factory,
            ITestOutputHelper testOutputHelper)
        {
            this.factory = factory;
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task SignInAfterSingOn()
        { 
            using var httpClient = factory.CreateClient();

            using var signOnResponse = await httpClient.PostAsync(
                "account", 
                JsonContent.Create(new { login = "Test", password = "qwerty123" }));

            signOnResponse.IsSuccessStatusCode.Should().BeTrue();
            var createdUser = await signOnResponse.Content.ReadFromJsonAsync<User>();


            using var signInResponse = await httpClient.PostAsync(
                "account/signin",
                JsonContent.Create(new { login = "Test", password = "qwerty123" }));

            signInResponse.IsSuccessStatusCode.Should().BeTrue();
            signInResponse.Headers.Should().ContainKey("TFA-Auth-Token");
            testOutputHelper.WriteLine(string.Join(Environment.NewLine,
                signInResponse.Headers.Select(h => $"{h.Key} {string.Join(", ", h.Value)}")));

            var signedIndUser = await signInResponse.Content.ReadFromJsonAsync<User>();

            signedIndUser.Should()
                .NotBeNull().And
                .BeEquivalentTo(createdUser);
        }
    }
}
