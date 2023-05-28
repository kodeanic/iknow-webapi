using System.Security.Claims;
using Application.Common.Dto;
using Application.Requests.Exercises.Commands;
using Application.Requests.Exercises.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/exercises")]
public class ExercisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExercisesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("get-exercise/{subtopicId:int}")]
    [Authorize]
    public async Task<ExerciseDto> GetExercise(int subtopicId)
    {
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone)!;
        return await _mediator.Send(new GetExerciseQuery(subtopicId, userPhone));
    }
    
    [HttpPost("check-answer/{exerciseId:int}")]
    [Authorize]
    public async Task<bool> CheckAnswer(int exerciseId, [FromBody] GetAnswerDto answer)
    {
        var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone)!;
        return await _mediator.Send(new CheckAnswerCommand(exerciseId, userPhone, answer.Answer));
    }

    [HttpGet("get-explanation/{exerciseId:int}")]
    [Authorize]
    public async Task<ExplanationDto> GetExerciseExplanation(int exerciseId)
    {
        return await _mediator.Send(new GetExerciseExplanationQuery { ExerciseId = exerciseId });
    }
}
