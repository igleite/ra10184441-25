using FluentValidation;

namespace Tenant.Application.Requests.Plans;

public sealed class UpdatePlanRequestValidator : AbstractValidator<UpdatePlanRequest>
{
    public UpdatePlanRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.MaxUsers).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxClients).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxTickets).GreaterThanOrEqualTo(0);
    }
}


