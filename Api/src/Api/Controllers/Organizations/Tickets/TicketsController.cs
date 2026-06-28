using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Interfaces;
using Tickets.Application.Commands.Tickets;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Queries.Tickets;
using Tickets.Application.Requests.Tickets;

namespace Api.Controllers.Organizations.Tickets;

[ApiController]
[Route("api/organizations/{organizationId}/tickets")]
public class TicketsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArtifactRepository _artifactRepository;
    private readonly ICustomerArtifactRepository _customerArtifactRepository;
    private readonly ITicketRepository _ticketRepository;

    public TicketsController(
        IMediatorHandler mediator,
        IOrganizationRepository organizationRepository,
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IArtifactRepository artifactRepository,
        ICustomerArtifactRepository customerArtifactRepository,
        ITicketRepository ticketRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _artifactRepository = artifactRepository;
        _customerArtifactRepository = customerArtifactRepository;
        _ticketRepository = ticketRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetTicketByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<TicketDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTicketByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<TicketDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<TicketDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var customer = await _customerRepository.GetByIdAsync(organizationId,request.CustomerId);
        if (customer is null || customer.OrganizationId != organizationId)
            return Result.Factory<TicketDto>.Error("O cliente não existe!", StatusCodes.Status404NotFound);

        var artifact = await _artifactRepository.GetByIdAsync(organizationId, request.ArtifactId);
        if (artifact is null)
            return Result.Factory<TicketDto>.Error("O artefato alvo não existe!", StatusCodes.Status404NotFound);

        var customerArtifact = await _customerArtifactRepository.GetByCustomerIdAndArtifactIdAsync(
            organizationId, request.CustomerId, request.ArtifactId);
        if (customerArtifact is null)
            return Result.Factory<TicketDto>.Error("O artefato não está vinculado ao cliente informado!", StatusCodes.Status400BadRequest);

        var user = await _userRepository.GetByIdAsync(request.CreatedByUserId);
        if (user is null)
            return Result.Factory<TicketDto>.Error("O usuário não existe!", StatusCodes.Status404NotFound);

        var command = new CreateTicketCommand(
            organizationId,
            request.CustomerId,
            request.ArtifactId,
            request.ClassificationId,
            request.CreatedByUserId,
            request.Description);

        var result = await _mediator.SendCommand<CreateTicketCommand, TicketDto>(command, cancellationToken);
        return Result.Factory<TicketDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(organizationId, id);
        if (ticket is null)
            return Result.Factory<TicketDto>.Error("O ticket não existe!", StatusCodes.Status404NotFound);

        var artifact = await _artifactRepository.GetByIdAsync(organizationId, request.ArtifactId);
        if (artifact is null)
            return Result.Factory<TicketDto>.Error("O artefato alvo não existe!", StatusCodes.Status404NotFound);

        var customerArtifact = await _customerArtifactRepository.GetByCustomerIdAndArtifactIdAsync(
            organizationId, ticket.CustomerId, request.ArtifactId);
        if (customerArtifact is null)
            return Result.Factory<TicketDto>.Error("O artefato não está vinculado ao cliente do ticket!", StatusCodes.Status400BadRequest);

        var command = new UpdateTicketCommand(
            organizationId,
            id,
            request.StatusId,
            request.ClassificationId,
            request.ArtifactId,
            request.AllocationCenter,
            request.Description,
            request.Resolution,
            request.RowVersion);

        var result = await _mediator.SendCommand<UpdateTicketCommand, TicketDto>(command, cancellationToken);
        return Result.Factory<TicketDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteTicketCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteTicketCommand, TicketDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}
