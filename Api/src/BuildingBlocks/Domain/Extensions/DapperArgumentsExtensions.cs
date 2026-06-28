using Dapper;

namespace BuildingBlocks.Domain.Extensions;

public static class DapperArgumentsExtensions
{
    public static DbString ToChar(this string value, int length)
    {
        return new DbString()
        {
            Value = value,
            Length = length,
            IsAnsi = true,
            IsFixedLength = true
        };
    }

    public static DbString ToNChar(this string value, int length)
    {
        return new DbString()
        {
            Value = value,
            Length = length,
            IsAnsi = false,
            IsFixedLength = true
        };
    }

    public static DbString ToVarChar(this string value, int length)
    {
        return new DbString()
        {
            Value = value,
            Length = length,
            IsAnsi = true,
            IsFixedLength = false
        };
    }

    public static DbString ToNVarChar(this string value, int length)
    {
        return new DbString()
        {
            Value = value,
            Length = length,
            IsAnsi = false,
            IsFixedLength = false
        };
    }
}
