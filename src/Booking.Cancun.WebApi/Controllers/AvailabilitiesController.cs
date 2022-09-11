using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Cancun.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class AvailabilitiesController : ApiBaseController
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilitiesController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resul = await _availabilityService.GetRoomAvailability(1);
            return Ok(resul);
        }
    }
}
