using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Exercises.Commands;

public class CheckAnswerCommand : IRequest<bool>
{
    public int ExerciseId { get; set; }

    public string Phone { get; set; }

    public string Answer { get; set; }

    public CheckAnswerCommand(int exerciseId, string phone, string answer)
    {
        ExerciseId = exerciseId;
        Phone = phone;
        Answer = answer;
    }
}

public class CheckAnswerCommandHandler : IRequestHandler<CheckAnswerCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public CheckAnswerCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(CheckAnswerCommand request, CancellationToken cancellationToken)
    {
        var phone = string.Join("", request.Phone.Where(char.IsDigit));

        var user = await _context.Users
            .Where(u => u.Phone == phone)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException("Пользователя не существует");

        var rightAnswer = await _context.Exercises
            .Where(e => e.Id == request.ExerciseId)
            .Select(e => e.Answer)
            .SingleAsync(cancellationToken);

        if (!rightAnswer.Equals(request.Answer))
            return false;

        var subtopic = await _context.Subtopics
            .Include(s => s.Exercises)
            .Where(s => s.Exercises.FirstOrDefault(e => e.Id == request.ExerciseId) != null)
            .SingleAsync(cancellationToken);

        var progress = await _context.Progresses
            .Include(p=>p.User)
            .Include(p => p.Subtopic)
            .Where(p => p.Subtopic.Id == subtopic.Id)
            .SingleAsync(cancellationToken);

        progress.CompletedExercises++;

        if (progress.CompletedExercises >= subtopic.Exercises.Count)
        {
            var subtopics = await _context.Topics
                .Include(t => t.Subtopics)
                .Where(t => t.Subtopics.FirstOrDefault(s => s.Id == progress.Subtopic.Id) != null)
                .Select(t => t.Subtopics)
                .SingleAsync(cancellationToken);

            var nextIndex = subtopics.FindIndex(s => s.Id == progress.Subtopic.Id) + 1;

            var nextSubtopic = await _context.Progresses
                .Include(p => p.User)
                .Include(p => p.Subtopic)
                .Where(p => p.Subtopic.Id == subtopics[nextIndex].Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (nextSubtopic != null) nextSubtopic.IsOpen = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}