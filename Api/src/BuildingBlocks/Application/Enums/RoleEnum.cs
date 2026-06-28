namespace BuildingBlocks.Application.Enums;

public static class Policies
{
    public const string SuperAdmin = nameof(SuperAdmin);
    public const string Onboarding = nameof(Onboarding);
    public const string OrganizationOwner = nameof(OrganizationOwner);
    public const string OrganizationMember = nameof(OrganizationMember);
    public const string ClientAdmin = nameof(ClientAdmin);
    public const string ClientMember = nameof(ClientMember);

    public const string Default = nameof(Default);
}

public sealed class RoleBaseEnum
{
    private readonly Guid _id;
    private readonly string _role;

    public RoleBaseEnum(Guid id, string role)
    {
        _id = id;
        _role = role;
        _all.Add(this);
    }

    public Guid Id => _id;
    public string Role => _role;

    private static readonly List<RoleBaseEnum> _all = new();

    public static RoleBaseEnum From(Guid id, string role)
    {
        return _all.First(x => x.Id == id && string.Equals(x.Role, role, StringComparison.OrdinalIgnoreCase));
    }

    public static RoleBaseEnum? FindById(Guid id) =>
        _all.FirstOrDefault(x => x.Id == id);
}

public static class RoleEnum
{
    public static readonly RoleBaseEnum SuperAdmin = new(Guid.Parse("8f42805c-d971-4fdb-b099-d69c6dc6b2a4"), nameof(SuperAdmin));
    public static readonly RoleBaseEnum Onboarding = new(Guid.Parse("34de239d-221a-4345-97ac-80e9fcfa5b85"), nameof(Onboarding));
    public static readonly RoleBaseEnum OrganizationOwner = new(Guid.Parse("e597e22c-e22e-4849-928c-e919717d2b8c"), nameof(OrganizationOwner));
    public static readonly RoleBaseEnum OrganizationMember = new(Guid.Parse("dc8c024b-69e2-4800-af50-f0fbd690279d"), nameof(OrganizationMember));
    public static readonly RoleBaseEnum ClientAdmin = new(Guid.Parse("7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5"), nameof(ClientAdmin));
    public static readonly RoleBaseEnum ClientMember = new(Guid.Parse("11fe3aaa-d36d-4cae-9bce-7c5084360425"), nameof(ClientMember));

    public static readonly RoleBaseEnum Nullable = new(Guid.Empty, nameof(Nullable));
    public static readonly RoleBaseEnum Unchanged = new(Guid.Empty, nameof(Unchanged));
}