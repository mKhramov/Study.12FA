using Study.TFA.API.Authentication;
using Study.TFA.Domain.Authentication;

namespace Study.TFA.API.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAuthTokenStorage storage,
            IAuthenticationService authenticationService,
            IIdentityProvider identityProvider,
            CancellationToken cancellationToken)
        {
            var identity = storage.TryExtract(context, out var authToken) 
                ? await authenticationService.Authenticate(authToken, cancellationToken)
                : User.Guest;
            identityProvider.Current = identity;

            next?.Invoke(context);
        }
    }
}
