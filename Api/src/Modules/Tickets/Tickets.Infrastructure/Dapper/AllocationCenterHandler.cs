using Dapper;
using System.Data;
using Tickets.Domain.ValueObjects;

namespace Tickets.Infrastructure.Dapper;

public sealed class AllocationCenterHandler : SqlMapper.TypeHandler<AllocationCenter>
{
    public override void SetValue(IDbDataParameter parameter, AllocationCenter value)
    {
        parameter.Value = value.Value;
    }

    public override AllocationCenter Parse(object value)
    {
        return AllocationCenter.From((int)value);
    }
}
