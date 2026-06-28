using BuildingBlocks.Domain.Entities;

namespace Identity.Domain.Entities;

public class Session : Entity
{
    public Guid UserId { get; protected set; }
    public string Token { get; protected set; } = string.Empty;
    public DateTime ExpiresAt { get; protected set; }
    public DateTime LastUsedAt { get; protected set; }

    protected Session() { }

    public Session(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetUserId(Guid userId, DateTime updatedAt)
    {
        UserId = userId;
        SetUpdatedAt(updatedAt);
    }

    public void SetToken(string token, DateTime updatedAt)
    {
        Token = token;
        SetUpdatedAt(updatedAt);
    }

    public void SetExpiresAt(DateTime expiresAt, DateTime updatedAt)
    {
        ExpiresAt = expiresAt;
        SetUpdatedAt(updatedAt);
    }

    public void SetLastUsedAt(DateTime lastUsedAt, DateTime updatedAt)
    {
        LastUsedAt = lastUsedAt;
        SetUpdatedAt(updatedAt);
    }
}
