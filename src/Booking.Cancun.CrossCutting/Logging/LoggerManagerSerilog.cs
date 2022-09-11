using Microsoft.Extensions.Logging;

namespace Booking.Cancun.CrossCutting.Logging;

public class LoggerManagerSerilog : ILoggerManager
{
    private readonly ILogger<LoggerManagerSerilog> logger;

    public LoggerManagerSerilog(ILogger<LoggerManagerSerilog> logger)
    {
        this.logger = logger;
    }

    public void LogDebug(string message)
    {
        logger.LogDebug(message);
    }

    public void LogError(Exception ex)
    {
        logger.LogError(ex, ex.Message);
    }

    public void LogInfo(string message)
    {
        logger.LogInformation(message);
    }

    public void LogWarn(string message)
    {
        logger.LogWarning(message);
    }
}
