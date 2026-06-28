using FluentValidation;

namespace FeatureFlags.Application.Requests;

public sealed class CreateFeatureFlagRequestValidator : AbstractValidator<CreateFeatureFlagRequest>
{
    public CreateFeatureFlagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);
    }
}


