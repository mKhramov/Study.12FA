using Microsoft.AspNetCore.Mvc;
using Study.TFA.API.Models;
using Study.TFA.Domain.Authorization;
using Study.TFA.Domain.Exceptions;
using Study.TFA.Domain.UseCases.CreateTopic;
using Study.TFA.Domain.UseCases.GetForums;

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
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(Topic))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopic request,
            [FromServices] ICreateTopicUseCase useCase,
            CancellationToken cancellationToke)
        {
            try
            {
                var topic = await useCase.Execute(forumId, request.Title, cancellationToke);
                return CreatedAtRoute(nameof(GetForums), new Topic()
                {
                    Id = topic.Id,
                    CreatedAt = topic.CreatedAt,
                    Title = topic.Title,
                });
            }
            catch (Exception exception)
            {
                return exception switch
                {
                    IntentionManagerException => Forbid(),
                    ForumNotFoundException => StatusCode(StatusCodes.Status410Gone),
                    _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
        }
    }
}