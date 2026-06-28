using Dapper;
using System.Data;
using Tickets.Domain.ValueObjects;

namespace Tickets.Domain.Extensions;

public sealed class DapperStatusTypeHandler : SqlMapper.TypeHandler<StatusType>
{
    public override void SetValue(IDbDataParameter parameter, StatusType value)
    {
        parameter.Value = value.Value;
    }

    public override StatusType Parse(object value)
    {
        return StatusType.From((int)value);
    }
}