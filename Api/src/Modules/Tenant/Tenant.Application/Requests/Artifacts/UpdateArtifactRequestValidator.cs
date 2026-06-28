using FluentValidation;

namespace Tenant.Application.Requests.Artifacts;

public sealed class UpdateArtifactRequestValidator : AbstractValidator<UpdateArtifactRequest>
{
    public UpdateArtifactRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.RowVersion)
            .NotEmpty();
    }
}
