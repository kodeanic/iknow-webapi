using System.Security.Claims;
using Application.Common.Dto;
using Application.Requests.Topics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/topics")]
public class TopicsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TopicsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("get-topics/{subjectId:int}")]
    [Authorize]
    public async Task<List<TopicDto>> GetTopics(int subjectId)
    {
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone)!;
        return await _mediator.Send(new GetTopicsQuery(subjectId, userPhone));
    }
    
    [HttpGet("get-theory/{subtopicId:int}")]
    [Authorize]
    public async Task<TheoryDto> GetSubtopicTheory(int subtopicId)
    {
        return await _mediator.Send(new GetSubtopicTheoryQuery { SubtopicId = subtopicId });
    }
}
