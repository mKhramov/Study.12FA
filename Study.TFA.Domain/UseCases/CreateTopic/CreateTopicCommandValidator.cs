using FluentValidation;

namespace Study.TFA.Domain.UseCases.CreateTopic
{
    internal class CreateTopicCommandValidator: AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(x => x.ForumId).NotEmpty().WithErrorCode("Empty");
            RuleFor(x => x.Title).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode("Empty")
                .MaximumLength(100).WithErrorCode("TooLong");
        }
    }
}
