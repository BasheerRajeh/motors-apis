using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        public BadRequestObjectResult BadRequest(string? error)
            => BadRequest(new { Message = error });
        public NotFoundObjectResult NotFound(string? error)
            => NotFound(new { Message = error });
        public OkObjectResult Ok(string? message)
            => Ok(new { Message = message });
    }
}
