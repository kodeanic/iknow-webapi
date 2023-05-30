using Application.Common.Dto;
using Application.Common.Exceptions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Exercises.Queries;

public class GetExerciseQuery : IRequest<TaskDto>
{
    public int SubtopicId { get; set; }
    
    public string Phone { get; set; }
    
    public GetExerciseQuery(int subtopicId, string phone)
    {
        SubtopicId = subtopicId;
        Phone = phone;
    }
}

public class GetExerciseQueryHandler : IRequestHandler<GetExerciseQuery, TaskDto>
{
    private readonly IApplicationDbContext _context;

    public GetExerciseQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<TaskDto> Handle(GetExerciseQuery request, CancellationToken cancellationToken)
    {
        var phone = string.Join("", request.Phone.Where(char.IsDigit));

        var user = await _context.Users
            .Where(u => u.Phone == phone)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException("Пользователя не существует");

        var subtopicProgress = await _context.Progresses
            .Include(p => p.User)
            .Include(p => p.Subtopic)
            .Where(p => p.User.Id == user.Id && p.Subtopic.Id == request.SubtopicId)
            .Select(p => p.CompletedExercises)
            .SingleAsync(cancellationToken);

        var type = await _context.Subtopics
            .Where(s => s.Id == request.SubtopicId)
            .Select(s => s.Type)
            .SingleAsync(cancellationToken);

        TaskDto response;
        if (type == TaskType.Exercise)
        {
            response = await GetExercise(request.SubtopicId, subtopicProgress);
        }
        else
        {
            response = await GetConstellation(request.SubtopicId, subtopicProgress);
        }

        return response;
    }

    private async Task<TaskDto> GetExercise(int subtopicId, int progress)
    {
        var exercises = await _context.Subtopics
            .Where(s => s.Id == subtopicId)
            .Select(s => s.Exercises)
            .SingleAsync();
        
        if (progress >= exercises!.Count)
            throw new BadRequestException("Задания по данной теме закончились");

        var exercise = exercises.Skip(progress).First();

        return new TaskDto
        {
            Type = TaskType.Exercise.ToString().ToLower(),
            Exercise = new ExerciseDto
            {
                Id = exercise.Id,
                Question = exercise.Question,
                Options = exercise.Options
            }
        };
    }
    
    private async Task<TaskDto> GetConstellation(int subtopicId, int progress)
    {
        var constellations = await _context.Subtopics
            .Where(s => s.Id == subtopicId)
            .Select(s => s.Constellations)
            .SingleAsync();
        
        if (progress >= constellations!.Count)
            throw new BadRequestException("Задания по данной теме закончились");

        var constellation = constellations.Skip(progress).First();
        constellation = await _context.Constellations
            .Where(x => x.Id == constellation.Id)
            .Include(x => x.Stars)
            .Include(x => x.Lines)
            .SingleAsync();

        var response = new ConstellationDto
        {
            Id = constellation.Id,
            Name = constellation.Name,
            Stars = new List<StarDto>(),
            Lines = new List<LineDto>()
        };

        foreach (var star in constellation.Stars)
        {
            response.Stars.Add(new StarDto
            {
                Number = star.Number,
                X = star.X,
                Y = star.Y,
                IsClicked = star.IsClicked
            });
        }

        foreach (var line in constellation.Lines)
        {
            response.Lines.Add(new LineDto
            {
                Number = line.Number,
                StarLeftNumber = line.StarLeftNumber,
                StarRightNumber = line.StarRightNumber
            });
        }
        
        return new TaskDto
        {
            Type = TaskType.Constellation.ToString().ToLower(),
            Constellation = response
        };
    }
}
