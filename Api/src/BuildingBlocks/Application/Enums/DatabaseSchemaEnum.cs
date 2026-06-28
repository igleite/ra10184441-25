namespace BuildingBlocks.Application.Enums;

public sealed class DatabaseSchemaBaseEnum
{
    private readonly string _schema;
    private readonly string _table;

    public DatabaseSchemaBaseEnum(string schema, string table)
    {
        _schema = schema;
        _table = table;
        _all.Add(this);
    }

    public string Schema => _schema;
    public string Table => _table;
    public string FullName => $"{_schema}.{_table}";

    public override string ToString() => FullName;

    // =========================
    // Lookup global
    // =========================
    private static readonly List<DatabaseSchemaBaseEnum> _all = new();

    public static DatabaseSchemaBaseEnum From(string schema, string table)
    {
        return _all.First(x =>
            string.Equals(x.Schema, schema, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Table, table, StringComparison.OrdinalIgnoreCase));
    }
}


public sealed class DatabaseSchemaEnum
{
    public sealed class SdpDpNew()
    {
        public static readonly DatabaseSchemaBaseEnum Organization = new("[tenants]", "[organization]");
        public static readonly DatabaseSchemaBaseEnum Customer = new("[tenants]", "[customer]");
        public static readonly DatabaseSchemaBaseEnum Plan = new("[tenants]", "[plan]");
        public static readonly DatabaseSchemaBaseEnum OrganizationPlan = new("[tenants]", "[organization_plan]");
        public static readonly DatabaseSchemaBaseEnum User = new("[tenants]", "[user]");
        public static readonly DatabaseSchemaBaseEnum CustomerUser = new("[tenants]", "[customer_user]");
        public static readonly DatabaseSchemaBaseEnum OrganizationUser = new("[tenants]", "[organization_user]");
        public static readonly DatabaseSchemaBaseEnum Team = new("[tenants]", "[team]");
        public static readonly DatabaseSchemaBaseEnum Role = new("[tenants]", "[role]");
        public static readonly DatabaseSchemaBaseEnum Ticket = new("[tickets]", "[ticket]");
        public static readonly DatabaseSchemaBaseEnum StatusReason = new("[tickets]", "[status_reason]");
        public static readonly DatabaseSchemaBaseEnum TicketClassification = new("[tickets]", "[ticket_classification]");
        public static readonly DatabaseSchemaBaseEnum Chat = new("[tickets]", "[chat]");
        public static readonly DatabaseSchemaBaseEnum FeatureFlag = new("[feature_flags]", "[feature_flag]");
        public static readonly DatabaseSchemaBaseEnum ArtifactType = new("[tenants]", "[artifact_type]");
        public static readonly DatabaseSchemaBaseEnum Artifact = new("[tenants]", "[artifact]");
        public static readonly DatabaseSchemaBaseEnum CustomerArtifact = new("[tenants]", "[customer_artifact]");
        public static readonly DatabaseSchemaBaseEnum Session = new("[identity]", "[session]");
        public static readonly DatabaseSchemaBaseEnum VerificationToken = new("[identity]", "[verification_token]");
    }
}