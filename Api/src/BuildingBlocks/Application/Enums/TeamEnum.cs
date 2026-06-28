namespace BuildingBlocks.Application.Enums;

public sealed class TeamBaseEnum
{
    private readonly string _code;
    private readonly string _name;

    public TeamBaseEnum(string code, string name)
    {
        _code = code;
        _name = name;
        _all.Add(this);
    }

    public string Code => _code;
    public string Name => _name;

    // =========================
    // Lookup global
    // =========================
    private static readonly List<TeamBaseEnum> _all = new();

    public static TeamBaseEnum From(string code, string name)
    {
        return _all.First(x => 
            string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)
        );
    }
}

public sealed class TeamEnum
{
    public static readonly TeamBaseEnum OrganizationAdmin = new("DEFAULT_TEAM", "Default Team");
}