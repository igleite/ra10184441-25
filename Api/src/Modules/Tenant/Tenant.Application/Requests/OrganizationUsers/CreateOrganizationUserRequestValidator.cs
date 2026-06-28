using FluentValidation;

namespace Tenant.Application.Requests.OrganizationUsers;

public sealed class CreateOrganizationUserRequestValidator : AbstractValidator<CreateOrganizationUserRequest>
{
    public CreateOrganizationUserRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TeamId).NotEmpty();
    }
}


