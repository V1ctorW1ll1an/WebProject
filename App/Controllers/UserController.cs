using Microsoft.AspNetCore.Mvc;
using App.Services;

namespace App.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController : ControllerBase
{
    public readonly IUserService _userService;

    public UserController(IUserService userUservice)
    {
        _userService = userUservice;
    }

    [HttpGet(Name = "GetUser")]
    public async Task<IActionResult> Get()
    {
        return Ok("all good");
    }
}
