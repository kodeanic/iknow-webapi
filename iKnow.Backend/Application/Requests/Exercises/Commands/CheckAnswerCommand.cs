using Application.Common.Exceptions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Exercises.Commands;

public class CheckAnswerCommand : IRequest<bool>
{
    public TaskType Type { get; set; }
    
    public int TaskId { get; set; }

    public string Phone { get; set; }

    public string Answer { get; set; }

    public CheckAnswerCommand(int exerciseId, string phone, string answer, string type)
    {
        TaskId = exerciseId;
        Phone = phone;
        Answer = answer;
        Type = (TaskType) Enum.Parse(typeof(TaskType), type);
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

        var response = false;
        if (request.Type == TaskType.Exercise)
        {
            response = await CheckForExercises(request);
        }
        else
        {
            if (request.Answer == "true")
            {
                response = await UpdateConstellation(request);
            }
        }
        return response;
    }
    
    private async Task<bool> CheckForExercises(CheckAnswerCommand request)
    {
        var rightAnswer = await _context.Exercises
            .Where(e => e.Id == request.TaskId)
            .Select(e => e.Answer)
            .SingleAsync();

        if (!rightAnswer.Equals(request.Answer))
            return false;

        var subtopic = await _context.Subtopics
            .Include(s => s.Exercises)
            .Where(s => s.Exercises!.FirstOrDefault(e => e.Id == request.TaskId) != null)
            .SingleAsync();

        var progress = await _context.Progresses
            .Include(p=>p.User)
            .Include(p => p.Subtopic)
            .Where(p => p.Subtopic.Id == subtopic.Id)
            .SingleAsync();

        progress.CompletedExercises++;

        if (progress.CompletedExercises >= subtopic.Exercises!.Count)
        {
            var subtopics = await _context.Topics
                .Include(t => t.Subtopics)
                .Where(t => t.Subtopics.FirstOrDefault(s => s.Id == progress.Subtopic.Id) != null)
                .Select(t => t.Subtopics)
                .SingleAsync();

            var nextIndex = subtopics.FindIndex(s => s.Id == progress.Subtopic.Id) + 1;

            var nextSubtopic = await _context.Progresses
                .Include(p => p.User)
                .Include(p => p.Subtopic)
                .Where(p => p.Subtopic.Id == subtopics[nextIndex].Id)
                .SingleOrDefaultAsync();

            if (nextSubtopic != null) nextSubtopic.IsOpen = true;
        }

        await _context.SaveChangesAsync();
        return true;
    }
    
    private async Task<bool> UpdateConstellation(CheckAnswerCommand request)
    {
        var subtopic = await _context.Subtopics
            .Include(s => s.Constellations)
            .Where(s => s.Constellations!
                .FirstOrDefault(c => c.Id == request.TaskId) != null)
            .SingleAsync();

        var progress = await _context.Progresses
            .Include(p=>p.User)
            .Include(p => p.Subtopic)
            .Where(p => p.Subtopic.Id == subtopic.Id)
            .SingleAsync();

        progress.CompletedExercises++;

        if (progress.CompletedExercises >= subtopic.Constellations!.Count)
        {
            var subtopics = await _context.Topics
                .Include(t => t.Subtopics)
                .Where(t => t.Subtopics.FirstOrDefault(s => s.Id == progress.Subtopic.Id) != null)
                .Select(t => t.Subtopics)
                .SingleAsync();

            var nextIndex = subtopics.FindIndex(s => s.Id == progress.Subtopic.Id) + 1;

            var nextSubtopic = await _context.Progresses
                .Include(p => p.User)
                .Include(p => p.Subtopic)
                .Where(p => p.Subtopic.Id == subtopics[nextIndex].Id)
                .SingleOrDefaultAsync();

            if (nextSubtopic != null) nextSubtopic.IsOpen = true;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
