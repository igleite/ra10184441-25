using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.CustomerUser;

public class DeleteCustomerUserCommandHandler : ICommandHandler<DeleteCustomerUserCommand, CustomerUserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerUserRepository _repository;

    public DeleteCustomerUserCommandHandler(IDateTimeProvider dateTimeProvider, ICustomerUserRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<CustomerUserDto> Handle(DeleteCustomerUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var customerUser = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (customerUser is null)
            throw AppException.NotFound($"A relação entre cliente e usuário não existe!");

        if (!customerUser.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A relação entre cliente e usuário foi modificada por outro usuário. Recarregue a página e tente novamente.");

        customerUser.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(customerUser);
        if (!success)
            throw AppException.BadRequest($"A relação entre cliente e usuário não foi deletada!");

        return CustomerUserMappings.ToDto(customerUser);
    }
}

