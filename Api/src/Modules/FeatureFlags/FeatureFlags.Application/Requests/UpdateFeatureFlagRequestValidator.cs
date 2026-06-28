using FluentValidation;

namespace FeatureFlags.Application.Requests;

public sealed class UpdateFeatureFlagRequestValidator : AbstractValidator<UpdateFeatureFlagRequest>
{
    public UpdateFeatureFlagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);
    }
}


