using Application.Requests.Users.Commands.CreateUser;
using Application.Requests.Users.Commands.DeleteUser;
using Application.Requests.Users.Commands.LoginUser;
using Application.Requests.Users.Queries.FindUser;
using Common.Models.Auth;
using Common.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Services.Auth;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtTokenService _jwtTokenService;

    public UserController(IMediator mediator, IJwtTokenService jwtTokenService) =>
        (_mediator, _jwtTokenService) = (mediator, jwtTokenService);

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (await _mediator.Send(new FindUserQuery(command.Email)) != null)
        {
            return BadRequest();
        }

        var user = await _mediator.Send(command);
        var token = _jwtTokenService.GenerateJwtToken(user);

        return Ok(new AuthenticationResult(token, user.Id));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (await _mediator.Send(new FindUserQuery(command.Email)) == null)
        {
            return BadRequest();
        }

        var user = await _mediator.Send(command);
        var token = _jwtTokenService.GenerateJwtToken(user);

        return Ok(new AuthenticationResult(token, user.Id));
    }

    [HttpDelete("delete")]
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
