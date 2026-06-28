using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.CustomerArtifact;

public class CreateCustomerArtifactCommandHandler : ICommandHandler<CreateCustomerArtifactCommand, CustomerArtifactDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerArtifactRepository _customerArtifactRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IArtifactRepository _artifactRepository;

    public CreateCustomerArtifactCommandHandler(
        IDateTimeProvider dateTimeProvider,
        ICustomerArtifactRepository customerArtifactRepository,
        ICustomerRepository customerRepository,
        IArtifactRepository artifactRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _customerArtifactRepository = customerArtifactRepository;
        _customerRepository = customerRepository;
        _artifactRepository = artifactRepository;
    }

    public async Task<CustomerArtifactDto> Handle(CreateCustomerArtifactCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var customer = await _customerRepository.GetByIdAsync(request.OrganizationId,request.CustomerId);
        if (customer is null || customer.OrganizationId != request.OrganizationId)
            throw AppException.NotFound($"O cliente não existe!");

        var artifact = await _artifactRepository.GetByIdAsync(request.OrganizationId, request.ArtifactId);
        if (artifact is null)
            throw AppException.NotFound($"O artefato não existe!");

        var exists = await _customerArtifactRepository.GetByCustomerIdAndArtifactIdAsync(request.OrganizationId, request.CustomerId, request.ArtifactId);
        if (exists != null)
            throw AppException.Conflict($"A relação entre cliente e artefato já existe!");

        var customerArtifact = new Domain.Entities.CustomerArtifact(Guid.NewGuid(), dateNow);
        customerArtifact.SetCustomerId(request.CustomerId, dateNow);
        customerArtifact.SetArtifactId(request.ArtifactId, dateNow);

        var success = await _customerArtifactRepository.CreateAsync(customerArtifact);
        if (!success)
            throw AppException.BadRequest($"A relação entre cliente e artefato não foi inserida!");

        return customerArtifact.ToDto();
    }
}
