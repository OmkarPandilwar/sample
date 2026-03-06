using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetCustomers()
    {
        return Ok("This is protected customer data");
    }
}