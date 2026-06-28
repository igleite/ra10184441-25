using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Customer;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(IDateTimeProvider dateTimeProvider, ICustomerRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var documentExists = await _repository.GetByDocumentAsync(request.OrganizationId, request.Document);
        if (documentExists != null)
            throw AppException.Conflict($"O cliente {request.Name} com o documento {request.Document} já existe!");

        var customer = new Domain.Entities.Customer(Guid.NewGuid(), dateNow, request.OrganizationId);
        customer.SetName(request.Name, dateNow);
        customer.SetDocument(request.Document, dateNow);

        var success = await _repository.CreateAsync(customer);
        if (!success)
            throw AppException.BadRequest($"O cliente {request.Name} com o documento {request.Document} não foi inserido!");

        return CustomerMappings.ToDto(customer);
    }
}

