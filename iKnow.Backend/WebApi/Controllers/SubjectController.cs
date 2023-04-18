using System.Security.Claims;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Requests.Subjects.Queries;
using Application.Requests.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/subjects")]
public class SubjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetTopics([FromBody] GetTopicsQuery command)
    {
        
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
        var user = userPhone is null ?
            throw new BadRequestException("Ошибка валидации токена") :
            await _mediator.Send(new FindUserQuery(userPhone));
        
        var topics = await _mediator.Send(command);
        
        var progress = await _mediator.Send(new GetProgressQuery(user));

        var topicDto = new List<TopicDto>();
        
        foreach (var topic in topics)
        {
            var tasksCount = topic.Subtopics.Sum(subtopic => subtopic.Tasks);
            var doneCount = 0;

            var subtopicDto = new List<SubtopicDto>();
            foreach (var subtopic in topic.Subtopics)
            {
                var pr = progress.Single(p => p.Topic == subtopic);
                doneCount += pr.Number;
                
                subtopicDto.Add(new SubtopicDto
                {
                    Id = subtopic.Id,
                    Title = subtopic.Title,
                    Progress = pr.Number * 100 / subtopic.Tasks,
                    State = pr.State.ToString()
                });
            }
            
            topicDto.Add(new TopicDto
            {
                Title = topic.Title,
                Progress = doneCount * 100 / tasksCount,
                Subtopics = subtopicDto
            });
        }
        return Ok(topicDto);
    }
}
