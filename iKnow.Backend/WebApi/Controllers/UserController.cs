using Application.Requests.Users.Commands.CreateUser;
using Application.Requests.Users.Commands.DeleteUser;
using Application.Requests.Users.Commands.LoginUser;
using Application.Requests.Users.Commands.Refresh;
using Application.Requests.Users.Queries.FindUser;
using Application.Requests.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoginService _loginService;

    public UserController(IMediator mediator, ILoginService loginService)
    {
        _mediator = mediator;
        _loginService = loginService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _mediator.Send(command);
        var result = await _loginService.Login(user);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _mediator.Send(command);
        var result = await _loginService.Login(user);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
    {
        var result = await _loginService.Refresh(command.RefreshToken);

        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var userLoginData = User.FindFirstValue(ClaimTypes.Name);
        
        return Ok(await _mediator.Send(new FindUserQuery(userLoginData)));
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var userLoginData = User.FindFirstValue(ClaimTypes.Name);

        if (userLoginData == null)
        {
            return Ok("ERROORR");
        }
        
        await _mediator.Send(new DeleteUserCommand(userLoginData));

        return Ok();
    }
}
