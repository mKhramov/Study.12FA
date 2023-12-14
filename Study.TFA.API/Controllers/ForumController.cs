using Microsoft.AspNetCore.Mvc;
using Study.TFA.API.Models;
using Study.TFA.Domain.UseCases.CreateTopic;
using Study.TFA.Domain.UseCases.GetForums;
using Study.TFA.Domain.UseCases.GetTopics;

namespace Study.TFA.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="useCase">Use Case</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetForums))]
        [ProducesResponseType(200, Type = typeof(Forum[]))]
        public async Task<IActionResult> GetForums(
            [FromServices] IGetForumsUseCase useCase,
            CancellationToken cancellationToken)
        {
            var forums = await useCase.Execute(cancellationToken);
            return Ok(forums.Select(x => new Forum() 
            { 
                Id = x.Id, 
                Title = x.Title 
            }));
        }

        [HttpPost("{forumId:guid}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(Topic))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopic request,
            [FromServices] ICreateTopicUseCase useCase,
            CancellationToken cancellationToken)
        {
            var command = new CreateTopicCommand(forumId, request.Title);
            var topic = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(GetForums), new Topic()
            {
                Id = topic.Id,
                CreatedAt = topic.CreatedAt,
                Title = topic.Title,
            });
        }

        [HttpGet("{forumId:guid}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(410)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTopics(
            [FromRoute] Guid forumId,
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromServices] IGetTopicsUseCase useCase,
            CancellationToken cancellationToken)
        {
            var query = new GetTopicsQuery(forumId, skip, take);
            var topics = await useCase.Execute(query, cancellationToken);

            return Ok(new
            {
                resources = topics.resources.Select(x => new Topic()
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Title = x.Title
                }),
                topics.totalCount
            });
        }
    }
}