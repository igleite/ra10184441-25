using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.Customer;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerRepository _repository;

    public UpdateCustomerCommandHandler(IDateTimeProvider dateTimeProvider, ICustomerRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var documentExists = await _repository.GetByDocumentAsync(request.OrganizationId, request.Document);
        if (documentExists != null && documentExists.Id != request.Id)
            throw AppException.Conflict($"O cliente {request.Name} com o documento {request.Document} já existe!");

        var customer = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (customer is null)
            throw AppException.NotFound($"O cliente não existe!");

        if (!customer.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O cliente foi modificado por outro usuário. Recarregue a página e tente novamente.");

        customer.SetName(request.Name, dateNow);
        customer.SetDocument(request.Document, dateNow);

        var success = await _repository.UpdateAsync(customer);
        if (!success)
            throw AppException.BadRequest($"O cliente {request.Name} com o documento {request.Document} não foi atualizado!");

        return CustomerMappings.ToDto(customer);
    }
}

