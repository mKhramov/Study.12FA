using FluentValidation;
using Study.TFA.Domain.Exceptions;

namespace Study.TFA.Domain.UseCases.SignIn
{
    internal class SignInCommandValidator: AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator(
            ISignInStorage storage)
        {
            RuleFor(c => c.Login).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
                //.MustAsync(async (_, login, cancellationToken) => 
                //{
                //    var recognisedUser = await storage.FindUser(login, cancellationToken);
                //    return recognisedUser is not null;
                //});

            RuleFor(c => c.Password)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }
    }
}
