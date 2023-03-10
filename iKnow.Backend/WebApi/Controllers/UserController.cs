using Application.Requests.Users.Commands.CreateUser;
using Application.Requests.Users.Commands.DeleteUser;
using Application.Requests.Users.Commands.UpdateUser;
using Application.Requests.Users.Queries.Dto;
using Application.Requests.Users.Queries.GetAllUsers;
using Application.Requests.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller //ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetAll() =>
        await _mediator.Send(new GetAllUsersQuery());
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(int id) =>
        await _mediator.Send(new GetUserQuery(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        /*
        var objectUrl = Url.Action(nameof(Get), new { id = userId });
        return Created(objectUrl!, userId);
        */
        return Ok(userId);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));

        return NoContent();
    }
}
