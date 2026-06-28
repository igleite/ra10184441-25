using DbUp.Engine.Output;
using Serilog;

namespace DatabaseMigrator;

internal sealed class SerilogUpgradeLog(ILogger logger) : IUpgradeLog
{
    public void WriteInformation(string format, params object[] args) =>
        logger.Information(format, args);

    public void WriteError(string format, params object[] args) =>
        logger.Error(format, args);

    public void WriteWarning(string format, params object[] args) =>
        logger.Warning(format, args);
}
