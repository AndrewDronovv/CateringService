using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
public class UsersController : ControllerBase
{
    private readonly IUserService _userSerivce;

    public UsersController(IUserService userSerivce)
    {
        _userSerivce = userSerivce;
    }

    [HttpPost("api/users")]
    public async Task<ActionResult<UserViewModel>> CreateUserAsync([FromBody] AddUserRequest request)
    {
        var createdUser = await _userSerivce.CreateUserAsync(request);

        return Ok(createdUser);
    }
}