using Booking.Cancun.Domain.Logging;
using Booking.Cancun.UnitTests.Utils.Producers;
using DotNetCore.CAP;
using Newtonsoft.Json;

namespace Booking.Cancun.UnitTests.Producers;

public class KafkaProducerTests
{
    private readonly ILoggerManager _logger;
    private readonly ICapPublisher _kafka;

    private KafkaProducerChild _kafkaProducerChild;

    public KafkaProducerTests()
    {
        _logger = Substitute.For<ILoggerManager>();
        _kafka = Substitute.For<ICapPublisher>();

        _kafkaProducerChild = new KafkaProducerChild(_logger, _kafka);
    }

    [Fact]
    public async Task CancelBookingOrder_WhenExists_MustCallRepositoryDelete()
    {
        // Arrange
        var publishModel = new KafkaProducerChildMock{
            Message = "message"
        };

        var stringMessage = JsonConvert.SerializeObject(publishModel);

        // Act
        await _kafkaProducerChild.PublishAsync(publishModel);

        // Assert
        await _kafka.Received(1).PublishAsync(nameof(KafkaProducerChild), stringMessage);
    }
}
