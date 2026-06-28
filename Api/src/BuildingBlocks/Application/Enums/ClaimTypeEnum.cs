namespace BuildingBlocks.Application.Enums;

public sealed class ClaimTypeBaseEnum
{
    private readonly string _type;

    public ClaimTypeBaseEnum(string type)
    {
        _type = type;
        _all.Add(this);
    }

    public string Type => _type;

    private static readonly List<ClaimTypeBaseEnum> _all = new();

    public static ClaimTypeBaseEnum From(string type) =>
        _all.First(x => string.Equals(x.Type, type, StringComparison.OrdinalIgnoreCase));

    public static ClaimTypeBaseEnum? FindByType(string type) =>
        _all.FirstOrDefault(x => string.Equals(x.Type, type, StringComparison.OrdinalIgnoreCase));
}

public static class ClaimTypeEnum
{
    public static readonly ClaimTypeBaseEnum Role = new("role");
    public static readonly ClaimTypeBaseEnum UserId = new("user_id");
    public static readonly ClaimTypeBaseEnum OrganizationId = new("organization_id");
}
