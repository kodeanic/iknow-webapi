using Application.Requests.Constellations.Queries;
using Domain.Entities.Constellations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/constellations")]
public class ConstellationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConstellationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<Constellation> Get(int id)
    {
        return await _mediator.Send(new GetConstellationQuery(id));
    }
}