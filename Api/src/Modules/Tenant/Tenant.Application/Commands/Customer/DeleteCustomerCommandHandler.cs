using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.Customer;

public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand, CustomerDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerRepository _repository;

    public DeleteCustomerCommandHandler(IDateTimeProvider dateTimeProvider, ICustomerRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<CustomerDto> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var customer = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (customer is null)
            throw AppException.NotFound($"O cliente não existe!");

        if (!customer.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O cliente foi modificado por outro usuário. Recarregue a página e tente novamente.");

        customer.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(customer);
        if (!success)
            throw AppException.BadRequest($"O cliente não foi deletado!");

        return CustomerMappings.ToDto(customer);
    }
}

