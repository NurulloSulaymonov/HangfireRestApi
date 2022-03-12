using Microsoft.AspNetCore.Mvc;
using webApi.Services;

namespace webApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController: ControllerBase
{
    public HomeController(IJobTestService jobService)
    {
    }
    
    
}