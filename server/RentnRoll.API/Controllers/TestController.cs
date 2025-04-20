using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Test endpoint is working!");
    }
}