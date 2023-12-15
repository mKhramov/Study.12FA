using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Study.TFA.API.Models;
using Study.TFA.Domain.UseCases.CreateForum;
using Study.TFA.Domain.UseCases.CreateTopic;
using Study.TFA.Domain.UseCases.GetForums;
using Study.TFA.Domain.UseCases.GetTopics;

namespace Study.TFA.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {
        [HttpPost(Name = nameof(CreateForum))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(201, Type = typeof(Forum))]
        public async Task<IActionResult> CreateForum(
            Guid forumId,
            [FromBody] CreateForum request,
            [FromServices] ICreateForumUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var command = new CreateForumCommand(request.Title);
            var forum = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(GetForums), mapper.Map<Forum>(forum));
        }


        [HttpGet(Name = nameof(GetForums))]
        [ProducesResponseType(200, Type = typeof(Forum[]))]
        public async Task<IActionResult> GetForums(
            [FromServices] IGetForumsUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var forums = await useCase.Execute(cancellationToken);
            return Ok(forums.Select(mapper.Map<Forum>));
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
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var command = new CreateTopicCommand(forumId, request.Title);
            var topic = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(GetForums), mapper.Map<Topic>(topic));
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
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var query = new GetTopicsQuery(forumId, skip, take);
            var (resources, totalCount) = await useCase.Execute(query, cancellationToken);

            return Ok(new { resources = resources.Select(mapper.Map<Topic>), totalCount });
        }
    }
}