using Application.Requests.Users.Commands;
using Application.Requests.Users.Queries;
using Application.Requests.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Common.Dto;
using Common.Models.Auth;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoginService _loginService;

    public UsersController(IMediator mediator, ILoginService loginService)
    {
        _mediator = mediator;
        _loginService = loginService;
    }

    [HttpPost("register")]
    public async Task<AuthenticationResult> Register([FromBody] CreateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return await _loginService.Login(user);
    }

    [HttpPost("login")]
    public async Task<AuthenticationResult> Login([FromBody] LoginUserCommand command)
    {
        var user = await _mediator.Send(command);
        return await _loginService.Login(user);
    }

    [HttpPost("refresh")]
    public async Task<AuthenticationResult> Refresh([FromBody] RefreshCommand command) =>
        await _loginService.Refresh(command.RefreshToken);

    [HttpGet]
    [Authorize]
    public async Task<UserDto> Get()
    {
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone)!;
        return await _mediator.Send(new FindUserQuery(userPhone));
    }

    [HttpDelete]
    [Authorize]
    public async Task Delete()
    {
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone)!;
        await _mediator.Send(new DeleteUserCommand(userPhone));
    }
    
    [HttpPut]
    [Authorize]
    public async Task<AuthenticationResult> Update([FromBody] UpdateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return await _loginService.Login(user);
    }
}
