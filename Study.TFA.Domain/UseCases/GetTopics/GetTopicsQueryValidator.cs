using FluentValidation;
using Study.TFA.Domain.Exceptions;

namespace Study.TFA.Domain.UseCases.GetTopics
{
    internal class GetTopicsQueryValidator: AbstractValidator<GetTopicsQuery>
    {
        public GetTopicsQueryValidator()
        {
            RuleFor(x => x.ForumId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
            RuleFor(x => x.Skip).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCode.Invalid);
            RuleFor(x => x.Take).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCode.Invalid);
        }
    }
}
