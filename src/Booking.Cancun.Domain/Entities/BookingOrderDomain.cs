using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Entities.Contracts;
using Booking.Cancun.Domain.Enums;
using Booking.Cancun.Domain.Exceptions;
using Booking.Cancun.Domain.Notifications;
using Newtonsoft.Json;

namespace Booking.Cancun.Domain.Entities;

public class BookingOrderDomain
{
    [JsonProperty]
    public Guid Id { get; private set; }

    [JsonProperty]
    public DateTime StartDate { get; private set; }

    [JsonProperty]
    public DateTime EndDate { get; private set; }

    [JsonProperty]
    public int RoomNumber { get; private set; }

    [JsonProperty]
    public string? Email { get; private set; }

    [JsonProperty]
    public EBookingOrderStatus Status { get; private set; }

    [JsonProperty]
    public IList<BookingOrderStayDomain> Stays { get; private set; }

    [JsonConstructor]
    private BookingOrderDomain()
    {
        Stays = new List<BookingOrderStayDomain>();
    }

    public BookingOrderDomain(BookingOrderRequestDTO bookingOrderRequest)
    {
        Id = Guid.NewGuid();
        RoomNumber = 1;
        Email = bookingOrderRequest.Email;
        StartDate = bookingOrderRequest.StartDate.Date;
        EndDate = bookingOrderRequest.EndDate.Date;
        Status = EBookingOrderStatus.Requested;

        Stays = new List<BookingOrderStayDomain>();

        CheckCreateContract();
    }

    public void Booked()
    {
        Status = EBookingOrderStatus.Booked;
    }

    public void Denied()
    {
        Status = EBookingOrderStatus.Denied;
    }

    public void GenerateStays()
    {
        for (var date = StartDate.Date; date.Date <= EndDate.Date; date = date.AddDays(1))
        {
            Stays.Add(new BookingOrderStayDomain(Id, date));
        }
    }

    private void CheckCreateContract()
    {
        var validationResult = new BookingOrderDomainCreateContract().Validate(this);

        if (!validationResult.IsValid)
        {
            var notifications = validationResult
                .Errors
                .Select(x => new DomainNotification(x.PropertyName, x.ErrorMessage))
                .DistinctBy(x => new { x.Key, x.Value })
                .ToList();

            throw new DomainException(notifications);
        }
    }
}
