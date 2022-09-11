using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Cancun.WebApi.Controllers;

[ApiVersion("1.0")]
public class BookingsController : ApiBaseController
{
    private readonly IBookingOrderCreate _bookingOrderCreate;

    public BookingsController(IBookingOrderCreate bookingOrderCreate)
    {
        _bookingOrderCreate = bookingOrderCreate;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingOrderRequestDTO request)
    {
        var result = await _bookingOrderCreate.CreateBookingOrder(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("GET");
    }
}
