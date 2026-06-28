using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.CustomerUser;

public class CreateCustomerUserCommandHandler : ICommandHandler<CreateCustomerUserCommand, CustomerUserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICustomerUserRepository _repository;
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerUserCommandHandler(IDateTimeProvider dateTimeProvider, ICustomerUserRepository repository, ICustomerRepository customerRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerUserDto> Handle(CreateCustomerUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var customer = await _customerRepository.GetByIdAsync(request.OrganizationId, request.CustomerId);
        if (customer is null)
            throw AppException.NotFound($"O cliente não existe!");

        var customerUserExists = await _repository.GetByCustomerIdAndUserIdAsync(request.OrganizationId, request.CustomerId, request.UserId);
        if (customerUserExists != null)
            throw AppException.Conflict($"A relação entre cliente e usuário já existe!");

        var customerUser = new Domain.Entities.CustomerUser(Guid.NewGuid(), dateNow, request.OrganizationId);
        customerUser.SetCustomerId(request.CustomerId, dateNow);
        customerUser.SetUserId(request.UserId, dateNow);

        // sempre cadastrado como membro
        customerUser.SetRoleId(RoleEnum.ClientMember.Id, dateNow);

        var success = await _repository.CreateAsync(customerUser);
        if (!success)
            throw AppException.BadRequest($"A relação entre cliente e usuário não foi inserida!");

        return CustomerUserMappings.ToDto(customerUser);
    }
}

