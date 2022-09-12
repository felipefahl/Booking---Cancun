using AutoMapper;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Infraestructure.Data.MsSql.Entities;

namespace Booking.Cancun.Infraestructure.Data.MsSql.Mappings;

public class GeneralProfileData : Profile
{
    public GeneralProfileData()
    {
        CreateMap<BookingOrderDb, BookingOrderDomain>();
        CreateMap<BookingOrderStayDb, BookingOrderStayDomain>();

    }
}
