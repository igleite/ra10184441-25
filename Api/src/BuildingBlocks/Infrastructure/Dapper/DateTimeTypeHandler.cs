using Dapper;
using System.Data;

namespace BuildingBlocks.Infrastructure.Dapper;

public sealed class DateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override DateTime Parse(object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        if (value is DateTime dateTime)
            return dateTime;

        if (value is string str)
        {
            if (DateTime.TryParseExact(str, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var parsedDateTime))
                return parsedDateTime;

            if (DateTime.TryParse(str, out var parsedDateTime2))
                return parsedDateTime2;

            throw new DataException($"Cannot convert string '{str}' to DateTime. Expected format: 'yyyy-MM-dd HH:mm:ss'");
        }

        throw new DataException(string.Format("Cannot convert {0} to DateTime", value.GetType()));
    }

    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value.ToString("yyyy-MM-dd HH:mm:ss");
        parameter.DbType = DbType.String;
    }
}