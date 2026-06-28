using FluentValidation;

namespace Tenant.Application.Requests.ArtifactTypes;

public sealed class CreateArtifactTypeRequestValidator : AbstractValidator<CreateArtifactTypeRequest>
{
    public CreateArtifactTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
    }
}
