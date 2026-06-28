using FluentValidation;

namespace Tenant.Application.Requests.CustomerArtifacts;

public sealed class UpdateCustomerArtifactRequestValidator : AbstractValidator<UpdateCustomerArtifactRequest>
{
    public UpdateCustomerArtifactRequestValidator()
    {
        RuleFor(x => x.ArtifactId)
            .NotEmpty();

        RuleFor(x => x.RowVersion)
            .NotEmpty();
    }
}
