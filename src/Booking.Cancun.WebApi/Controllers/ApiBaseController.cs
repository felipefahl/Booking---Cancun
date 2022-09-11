using Microsoft.AspNetCore.Mvc;

namespace Booking.Cancun.WebApi.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/v{version:ApiVersion}/[controller]")]
public abstract class ApiBaseController : ControllerBase
{    
}
