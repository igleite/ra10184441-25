namespace Tickets.Application.Requests.Chats;

public record UpdateChatRequest(string Message, byte[] RowVersion);