using Application.Requests.Users.Commands;
using Application.Requests.Users.Queries;
using Application.Requests.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Common.Exceptions;

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
        var user = await _mediator.Send(command);
        return Ok(await _loginService.Login(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var user = await _mediator.Send(command);
        return Ok(await _loginService.Login(user));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshCommand command) =>
        Ok(await _loginService.Refresh(command.RefreshToken));

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
        
        return userPhone is null ?
            throw new BadRequestException("Ошибка валидации токена") :
            Ok(await _mediator.Send(new FindUserQuery(userPhone)));
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
        
        return userPhone is null ?
            throw new BadRequestException("Ошибка валидации токена") :
            Ok(await _mediator.Send(new DeleteUserCommand(userPhone)));
    }
}
