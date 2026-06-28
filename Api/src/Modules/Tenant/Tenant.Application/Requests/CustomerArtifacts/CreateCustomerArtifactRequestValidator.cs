using FluentValidation;

namespace Tenant.Application.Requests.CustomerArtifacts;

public sealed class CreateCustomerArtifactRequestValidator : AbstractValidator<CreateCustomerArtifactRequest>
{
    public CreateCustomerArtifactRequestValidator()
    {
        RuleFor(x => x.ArtifactId)
            .NotEmpty();
    }
}
