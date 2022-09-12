using Booking.Cancun.Domain.Logging;
using DotNetCore.CAP;
using Newtonsoft.Json;

namespace Booking.Cancun.Infraestructure.CrossCutting.Producers;

public abstract class KafkaProducer<T>
{
    protected readonly ILoggerManager _logger;
    private readonly ICapPublisher _kafka;

    private readonly string _topic;

    protected KafkaProducer(ILoggerManager logger,
        ICapPublisher kafka,
        string topic)
    {
        _logger = logger;
        _kafka = kafka;
        _topic = topic;
    }

    public async Task PublishAsync(T message)
    {
        try
        {
            var stringMessage = JsonConvert.SerializeObject(message);

            await _kafka.PublishAsync(_topic, stringMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex);
        }
    }
}
