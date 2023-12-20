using Microsoft.AspNetCore.Mvc;
using Study.TFA.API.Authentication;
using Study.TFA.API.Models;
using Study.TFA.Domain.UseCases.SignOn;
using Study.TFA.Domain.UseCases.SignIn;

namespace Study.TFA.API.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> SignOn(
            [FromBody] SignOn request,
            [FromServices] ISignOnUseCase useCase,
            CancellationToken cancellationToken)
        {
            var indentity = await useCase.Execute(new SignOnCommand(request.Login, request.Password), cancellationToken);
            return Ok(indentity);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(
            [FromBody] SignIn request,
            [FromServices] ISignInUseCase useCase,
            [FromServices] IAuthTokenStorage tokenStorage,
            CancellationToken cancellationToken)
        {
            var (indentity, token) = await useCase.Execute(
                new SignInCommand(request.Login, request.Password), cancellationToken);
            tokenStorage.Store(HttpContext, token);
            return Ok(indentity);
        }
    }
}
