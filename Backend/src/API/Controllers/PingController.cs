using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController
{
    [HttpGet]
    public string ActivePing()
    {
        Console.WriteLine("Keep Active");
        return "Keep active";
    }
}
