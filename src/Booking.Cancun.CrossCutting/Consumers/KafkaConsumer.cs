using DotNetCore.CAP;
using Newtonsoft.Json;

namespace Booking.Cancun.Infraestructure.CrossCutting.Consumers;

public abstract class KafkaConsumer<T> : ICapSubscribe
{

    protected KafkaConsumer()
    {
    }

    public abstract Task ConsumeMessageAsync(string stringMessage, CancellationToken cancellationToken);

    protected T ConvertMessage(string stringMessage)
    {
        var message = JsonConvert.DeserializeObject<T>(stringMessage,
                              new JsonSerializerSettings
                              {
                                  ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                              });

        return message;
    }
}
