using FluentValidation;

namespace Identity.Application.Requests;

public sealed class VerifyRequestValidator : AbstractValidator<VerifyRequest>
{
    public VerifyRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
