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
        var userEmail = User?.FindFirstValue(ClaimTypes.Email);

        if (userEmail == null)
        {
            return BadRequest();
        }

        return Ok(await _mediator.Send(new FindUserQuery(userEmail)));
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var userEmail = User?.FindFirstValue(ClaimTypes.Email);

        if (userEmail == null)
        {
            return BadRequest();
        }

        var user = await _mediator.Send(new FindUserQuery(userEmail));
        await _mediator.Send(new DeleteUserCommand(user.Id));

        return Ok();
    }
}
