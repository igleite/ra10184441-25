using FluentValidation;

namespace Tenant.Application.Requests.Teams;

public sealed class UpdateTeamRequestValidator : AbstractValidator<UpdateTeamRequest>
{
    public UpdateTeamRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.RoleId).NotEmpty();

        RuleFor(x => x.RowVersion)
            .NotEmpty();
    }
}
