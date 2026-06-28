using Dapper;
using System.Data;

namespace BuildingBlocks.Infrastructure.Dapper;

public sealed class DateTimeNullableTypeHandler : SqlMapper.TypeHandler<DateTime?>
{
    public override DateTime? Parse(object value)
    {
        if (value == null || value == DBNull.Value)
            return null;

        if (value is DateTime dateTime)
            return dateTime;

        if (value is string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            if (DateTime.TryParseExact(str, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var parsedDateTime))
                return parsedDateTime;

            if (DateTime.TryParse(str, out var parsedDateTime2))
                return parsedDateTime2;

            throw new DataException($"Cannot convert string '{str}' to DateTime?. Expected format: 'yyyy-MM-dd HH:mm:ss'");
        }

        throw new DataException(string.Format("Cannot convert {0} to DateTime?", value.GetType()));
    }

    public override void SetValue(IDbDataParameter parameter, DateTime? value)
    {
        if (value.HasValue)
        {
            parameter.Value = value.Value.ToString("yyyy-MM-dd HH:mm:ss");
            parameter.DbType = DbType.String;
        }
        else
        {
            parameter.Value = DBNull.Value;
        }
    }
}