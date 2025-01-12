﻿using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Jobs;

public interface IRequestedBookingOrderJob
{
    Task Send(BookingOrderDomain bookingOrderDomain);
}
