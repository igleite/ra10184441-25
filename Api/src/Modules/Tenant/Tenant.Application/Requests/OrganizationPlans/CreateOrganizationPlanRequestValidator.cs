using FluentValidation;

namespace Tenant.Application.Requests.OrganizationPlans;

public sealed class CreateOrganizationPlanRequestValidator : AbstractValidator<CreateOrganizationPlanRequest>
{
    public CreateOrganizationPlanRequestValidator()
    {
        RuleFor(x => x.PlanId).NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.MaxUsers).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxClients).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxTickets).GreaterThanOrEqualTo(0);
    }
}


