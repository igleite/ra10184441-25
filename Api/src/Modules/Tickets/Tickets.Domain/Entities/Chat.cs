using BuildingBlocks.Domain.Entities;

namespace Tickets.Domain.Entities;

public class Chat : SoftDeleteEntity
{
    public Guid TicketId { get; protected set; } = Guid.Empty;
    public Guid UserId { get; protected set; } = Guid.Empty;
    public string Message { get; protected set; } = string.Empty;

    protected Chat() { }

    public Chat(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetTicketId(Guid id, DateTime updatedAt)
    {
        TicketId = id;
        SetUpdatedAt(updatedAt);
    }

    public void SetUserId(Guid id, DateTime updatedAt)
    {
        UserId = id;
        SetUpdatedAt(updatedAt);
    }

    public void SetMessage(string message, DateTime updatedAt)
    {
        Message = message;
        SetUpdatedAt(updatedAt);
    }
}