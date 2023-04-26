using Application.Common.Dto;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Exercises.Queries;

public class GetExerciseQuery : IRequest<ExerciseDto>
{
    public int SubtopicId { get; set; }
    
    public string Phone { get; set; }
    
    public GetExerciseQuery(int subtopicId, string phone)
    {
        SubtopicId = subtopicId;
        Phone = phone;
    }
}

public class GetExerciseQueryHandler : IRequestHandler<GetExerciseQuery, ExerciseDto>
{
    private readonly IApplicationDbContext _context;

    public GetExerciseQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<ExerciseDto> Handle(GetExerciseQuery request, CancellationToken cancellationToken)
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

        var exercises = await _context.Subtopics
            .Where(s => s.Id == request.SubtopicId)
            .Select(s => s.Exercises)
            .SingleAsync(cancellationToken);
            

        if (subtopicProgress >= exercises.Count)
            throw new BadRequestException();

        var exercise = exercises.Skip(subtopicProgress).First();
        
        return new ExerciseDto
        {
            Id = exercise.Id,
            Question = exercise.Question,
            Options = exercise.Options
        };
    }
}
