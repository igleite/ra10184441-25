using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.CustomerUser;

public class UpdateCustomerUserCommandHandler : ICommandHandler<UpdateCustomerUserCommand, CustomerUserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerUserRepository _repository;

    public UpdateCustomerUserCommandHandler(IDateTimeProvider dateTimeProvider, ICustomerUserRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<CustomerUserDto> Handle(UpdateCustomerUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var customerUser = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (customerUser is null)
            throw AppException.NotFound($"A relação entre cliente e usuário não existe!");

        if (!customerUser.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A relação entre cliente e usuário foi modificada por outro usuário. Recarregue a página e tente novamente.");

        var customerUserExists = await _repository.GetByCustomerIdAndUserIdAsync(request.OrganizationId, request.CustomerId, request.UserId);
        if (customerUserExists != null && customerUserExists.Id != request.Id)
            throw AppException.Conflict($"A relação entre cliente e usuário já existe!");

        customerUser.SetCustomerId(request.CustomerId, dateNow);
        customerUser.SetUserId(request.UserId, dateNow);

        var success = await _repository.UpdateAsync(customerUser);
        if (!success)
            throw AppException.BadRequest($"A relação entre cliente e usuário não foi atualizada!");

        return CustomerUserMappings.ToDto(customerUser);
    }
}

