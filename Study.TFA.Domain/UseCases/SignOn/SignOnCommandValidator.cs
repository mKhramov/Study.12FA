using FluentValidation;
using Study.TFA.Domain.Exceptions;

namespace Study.TFA.Domain.UseCases.SignOn
{
    internal class SignOnCommandValidator: AbstractValidator<SignOnCommand>
    {
        public SignOnCommandValidator(
            ISignOnStorage storage)
        {
            RuleFor(c => c.Login).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(20).WithErrorCode(ValidationErrorCode.TooLong);
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
