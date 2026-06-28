using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.CustomerArtifact;

public class UpdateCustomerArtifactCommandHandler : ICommandHandler<UpdateCustomerArtifactCommand, CustomerArtifactDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerArtifactRepository _customerArtifactRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IArtifactRepository _artifactRepository;

    public UpdateCustomerArtifactCommandHandler(
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

    public async Task<CustomerArtifactDto> Handle(UpdateCustomerArtifactCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var customer = await _customerRepository.GetByIdAsync(request.OrganizationId, request.CustomerId);
        if (customer is null || customer.OrganizationId != request.OrganizationId)
            throw AppException.NotFound($"O cliente não existe!");

        var artifact = await _artifactRepository.GetByIdAsync(request.OrganizationId, request.ArtifactId);
        if (artifact is null)
            throw AppException.NotFound($"O artefato não existe!");

        var linkExists = await _customerArtifactRepository.GetByCustomerIdAndArtifactIdAsync(request.OrganizationId, request.CustomerId, request.ArtifactId);
        if (linkExists != null && linkExists.Id != request.Id)
            throw AppException.Conflict($"A relação entre cliente e artefato já existe!");

        var customerArtifact = await _customerArtifactRepository.GetByIdAsync(request.OrganizationId, request.Id);
        if (customerArtifact is null)
            throw AppException.NotFound($"A relação entre cliente e artefato não existe!");

        if (!customerArtifact.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A relação foi modificada por outro usuário. Recarregue a página e tente novamente.");

        customerArtifact.SetCustomerId(request.CustomerId, dateNow);
        customerArtifact.SetArtifactId(request.ArtifactId, dateNow);

        var success = await _customerArtifactRepository.UpdateAsync(customerArtifact);
        if (!success)
            throw AppException.BadRequest($"A relação entre cliente e artefato não foi atualizada!");

        return customerArtifact.ToDto();
    }
}
