using FluentValidation;
using Study.TFA.Domain.Exceptions;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    internal class CreateTopicCommandValidator: AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(x => x.ForumId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
            RuleFor(x => x.Title).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong);
        }
    }
}
