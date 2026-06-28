using FluentValidation;

namespace Tenant.Application.Requests.ArtifactTypes;

public sealed class UpdateArtifactTypeRequestValidator : AbstractValidator<UpdateArtifactTypeRequest>
{
    public UpdateArtifactTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.RowVersion)
            .NotEmpty();
    }
}
