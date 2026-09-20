using Microsoft.AspNetCore.Mvc;

namespace School.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    public class BaseController : ControllerBase
    {
    }
}
