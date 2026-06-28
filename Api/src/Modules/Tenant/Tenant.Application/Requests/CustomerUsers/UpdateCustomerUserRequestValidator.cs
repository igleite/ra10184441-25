using FluentValidation;

namespace Tenant.Application.Requests.CustomerUsers;

public sealed class UpdateCustomerUserRequestValidator : AbstractValidator<UpdateCustomerUserRequest>
{
    public UpdateCustomerUserRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}


