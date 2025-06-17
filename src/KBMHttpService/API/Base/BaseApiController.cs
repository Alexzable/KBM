using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.API.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
