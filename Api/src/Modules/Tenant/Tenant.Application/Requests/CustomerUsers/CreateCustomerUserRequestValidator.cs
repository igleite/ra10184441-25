using FluentValidation;

namespace Tenant.Application.Requests.CustomerUsers;

public sealed class CreateCustomerUserRequestValidator : AbstractValidator<CreateCustomerUserRequest>
{
    public CreateCustomerUserRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}


