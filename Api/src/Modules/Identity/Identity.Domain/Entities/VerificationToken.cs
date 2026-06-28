using BuildingBlocks.Domain.Entities;

namespace Identity.Domain.Entities;

public class VerificationToken : Entity
{
    public string Token { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;
    public DateTime ExpiresAt { get; protected set; }

    protected VerificationToken() { }

    public VerificationToken(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetToken(string token, DateTime updatedAt)
    {
        Token = token;
        SetUpdatedAt(updatedAt);
    }

    public void SetEmail(string email, DateTime updatedAt)
    {
        Email = email;
        SetUpdatedAt(updatedAt);
    }

    public void SetExpiresAt(DateTime expiresAt, DateTime updatedAt)
    {
        ExpiresAt = expiresAt;
        SetUpdatedAt(updatedAt);
    }
}
