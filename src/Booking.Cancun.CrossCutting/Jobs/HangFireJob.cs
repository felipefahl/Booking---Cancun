using Booking.Cancun.CrossCutting.Logging;
using Hangfire;
using Newtonsoft.Json;

namespace Booking.Cancun.Infraestructure.CrossCutting.Jobs;

public abstract class HangFireJob<T>
{
    private readonly ILoggerManager _logger;

    protected HangFireJob(ILoggerManager logger)
    {
        _logger = logger;
    }

    public abstract Task ProccessMessage(T message);
    public async Task ExecuteJob(string stringMessage)
    {
        var message = JsonConvert.DeserializeObject<T>(stringMessage,
                              new JsonSerializerSettings
                              {
                                  ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                              });

        await ProccessMessage(message);
    }

    public async Task Proccess(T message)
    {
        try
        {
            var stringMessage = JsonConvert.SerializeObject(message);

            var client = new BackgroundJobClient();

            client.Enqueue(() => ExecuteJob(stringMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex);
        }
    }
}
