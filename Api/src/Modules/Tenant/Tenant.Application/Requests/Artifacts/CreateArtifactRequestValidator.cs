using FluentValidation;

namespace Tenant.Application.Requests.Artifacts;

public sealed class CreateArtifactRequestValidator : AbstractValidator<CreateArtifactRequest>
{
    public CreateArtifactRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);
    }
}
