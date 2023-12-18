using FluentAssertions;
using Study.TFA.API.Models;
using System.Net.Http.Json;

namespace Study.TFA.E2E
{
    public class TopicEndpointShould : IClassFixture<ForumApiApplicationFactory>, IAsyncLifetime
    {
        private readonly ForumApiApplicationFactory factory;
        private readonly Guid forumId = Guid.Parse("01762205-0462-4577-B0F5-50222E5D2D2F");

        public TopicEndpointShould(ForumApiApplicationFactory factory)
        {
            this.factory = factory;
        }

        public Task DisposeAsync()
        {
            //var dbContext = factory.Services.GetRequiredService<ForumDbContext>();            
            //await dbContext.Forums.AddAsync(new Forum() { ForumId = forumId, Title = "Test me", });
            //await dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task InitializeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ReturnForbidden_WhenNotAuthenticated()
        {
            using var httpClient = factory.CreateClient();

            using var responseCreateForum = await httpClient.PostAsync("forums"
                , JsonContent.Create(new { title = "New Forum" }));
            responseCreateForum.EnsureSuccessStatusCode();
            
            var forum = await responseCreateForum.Content.ReadFromJsonAsync<Forum>();
            forum.Should().NotBeNull();

            using var responseCreateTopic = await httpClient.PostAsync($"forums/{forum!.Id}/topics"
                , JsonContent.Create(new { title = "New Topic" }));
            responseCreateTopic.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
