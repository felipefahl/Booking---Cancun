using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Cancun.WebApi.Controllers;

[ApiVersion("1.0")]
public class BookingsController : ApiBaseController
{
    private readonly IBookingOrderCreate _bookingOrderCreate;
    private readonly IBookingOrderUpdate _bookingOrderUpdate;
    private readonly IBookingOrderCancel _bookingOrderCancel;

    public BookingsController(IBookingOrderCreate bookingOrderCreate,
        IBookingOrderUpdate bookingOrderUpdate,
        IBookingOrderCancel bookingOrderCancel)
    {
        _bookingOrderCreate = bookingOrderCreate;
        _bookingOrderUpdate = bookingOrderUpdate;
        _bookingOrderCancel = bookingOrderCancel;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingOrderRequestDTO request)
    {
        var result = await _bookingOrderCreate.CreateBookingOrder(request);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] BookingOrderRequestDTO request)
    {
        await _bookingOrderUpdate.UpdateBookingOrder(id, request);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _bookingOrderCancel.CancelBookingOrder(id);

        return Ok();
    }
}
