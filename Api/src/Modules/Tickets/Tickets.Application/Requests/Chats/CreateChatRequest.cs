namespace Tickets.Application.Requests.Chats;

public record CreateChatRequest(Guid UserId, string Message);