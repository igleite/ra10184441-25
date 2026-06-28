using Dapper;
using System.Data;

namespace BuildingBlocks.Infrastructure.Dapper;

public sealed class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        if (value is Guid guid)
            return guid;

        if (value is string str)
            return Guid.Parse(str);

        if (value is byte[] bytes)
            return new Guid(bytes);

        throw new DataException(string.Format("Cannot convert {0} to Guid", value.GetType()));
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
        parameter.DbType = DbType.String;
    }
}
