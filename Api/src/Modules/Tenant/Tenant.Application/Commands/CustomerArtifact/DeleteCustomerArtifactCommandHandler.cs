using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.CustomerArtifact;

public class DeleteCustomerArtifactCommandHandler : ICommandHandler<DeleteCustomerArtifactCommand, CustomerArtifactDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerArtifactRepository _repository;

    public DeleteCustomerArtifactCommandHandler(IDateTimeProvider dateTimeProvider, ICustomerArtifactRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<CustomerArtifactDto> Handle(DeleteCustomerArtifactCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var customerArtifact = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (customerArtifact is null)
            throw AppException.NotFound($"A relação entre cliente e artefato não existe!");

        if (!customerArtifact.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A relação foi modificada por outro usuário. Recarregue a página e tente novamente.");

        customerArtifact.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(customerArtifact);
        if (!success)
            throw AppException.BadRequest($"A relação entre cliente e artefato não foi deletada!");

        return customerArtifact.ToDto();
    }
}
