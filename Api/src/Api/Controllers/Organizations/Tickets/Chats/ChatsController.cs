using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using BuildingBlocks.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.Commands.Chat;
using Tickets.Application.DTOs;
using Tickets.Application.Queries.Chat;
using Tickets.Application.Requests.Chats;
using Tickets.Application.Interfaces;
using Tenant.Application.Interfaces;

namespace Api.Controllers.Organizations.Tickets.Chats;

[ApiController]
[Route("api/organizations/{organizationId}/tickets/{ticketId}/chats")]
public class ChatsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public ChatsController(IMediatorHandler mediator, ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _mediator = mediator;
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromRoute] Guid ticketId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetChatByPageQuery(organizationId, ticketId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<ChatDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, [FromRoute] Guid ticketId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetChatByIdQuery(organizationId, ticketId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<ChatDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromRoute] Guid ticketId, [FromBody] CreateChatRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(organizationId, ticketId);
        if (ticket is null)
            return Result.Factory<ChatDto>.Error("O ticket não existe!", StatusCodes.Status404NotFound);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Factory<ChatDto>.Error("O usuário não existe!", StatusCodes.Status404NotFound);

        var command = new CreateChatCommand(organizationId, ticketId, request.UserId, request.Message);

        var result = await _mediator.SendCommand<CreateChatCommand, ChatDto>(command, cancellationToken);
        return Result.Factory<ChatDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, [FromRoute] Guid ticketId, Guid id, [FromBody] UpdateChatRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(organizationId, ticketId);
        if (ticket is null)
            return Result.Factory<ChatDto>.Error("O ticket não existe!", StatusCodes.Status404NotFound);

        var command = new UpdateChatCommand(organizationId, id, ticketId, request.Message, request.RowVersion);

        var result = await _mediator.SendCommand<UpdateChatCommand, ChatDto>(command, cancellationToken);
        return Result.Factory<ChatDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteChatCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteChatCommand, ChatDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

