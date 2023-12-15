using FluentValidation;
using Study.TFA.Domain.Exceptions;

namespace Study.TFA.Domain.UseCases.CreateForum
{
    internal class CreateForumCommandValidator: AbstractValidator<CreateForumCommand>
    {
        public CreateForumCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
        }
    }
}
