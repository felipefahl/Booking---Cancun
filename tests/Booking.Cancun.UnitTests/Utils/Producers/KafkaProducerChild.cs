using Booking.Cancun.Domain.Logging;
using Booking.Cancun.Infraestructure.CrossCutting.Producers;
using DotNetCore.CAP;

namespace Booking.Cancun.UnitTests.Utils.Producers;

public class KafkaProducerChild : KafkaProducer<KafkaProducerChildMock>
{
    public KafkaProducerChild(ILoggerManager logger, ICapPublisher kafka) : base(logger, kafka, nameof(KafkaProducerChild))
    {
    }
}

public class KafkaProducerChildMock
{
    public string Message { get; set; }
}
