using FluentValidation;

namespace Tenant.Application.Requests.OrganizationUsers;

public sealed class UpdateOrganizationUserRequestValidator : AbstractValidator<UpdateOrganizationUserRequest>
{
    public UpdateOrganizationUserRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TeamId).NotEmpty();
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}


