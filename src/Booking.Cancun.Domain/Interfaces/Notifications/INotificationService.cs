using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Notifications;

public interface INotificationService
{
    Task SendBookedNotification(BookingOrderDomain bookingOrderDomain);
    Task SendUpdatedNotification(BookingOrderDomain bookingOrderDomain);
    Task SendDeniedNotification(BookingOrderDomain bookingOrderDomain);
    Task SendUpdatedDeniedNotification(BookingOrderDomain bookingOrderDomain);
    Task SendCancelledNotification(BookingOrderDomain bookingOrderDomain);
}
