﻿using Application.Common.Dto;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Topics.Queries;

public class GetTopicsQuery : IRequest<List<TopicDto>>
{
    public int SubjectId { get; set; }

    public string Phone { get; set; }

    public GetTopicsQuery(int subjectId, string phone)
    {
        SubjectId = subjectId;
        Phone = phone;
    }
}

public class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, List<TopicDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTopicsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<TopicDto>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
    {
        var phone = string.Join("", request.Phone.Where(char.IsDigit));

        var user = await _context.Users
            .Where(u => u.Phone == phone)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException("Пользователя не существует");

        var subject = await _context.Subjects
            .Include(s => s.Topics)
            .Where(s => s.Id == request.SubjectId)
            .SingleOrDefaultAsync(cancellationToken);

        if (subject is null)
            throw new NotFoundException("Предмет не найден");
        
        var progress = await _context.Progresses
            .Include(p => p.User)
            .Include(p => p.Subtopic)
            .Where(p => p.User.Id == user.Id)
            .ToListAsync(cancellationToken);
        
        var topicsDto = new List<TopicDto>();

        foreach (var topic in subject.Topics)
        {
            var subtopics = await _context.Topics
                .Include(x => x.Subtopics)
                .ThenInclude(x => x.Exercises)
                .Include(x => x.Subtopics)
                .ThenInclude(x => x.Constellations)
                .Where(x => x.Id == topic.Id)
                .Select(x => x.Subtopics)
                .SingleAsync(cancellationToken);

            var topicTasksCount = subtopics
                .Sum(subtopic => subtopic.Exercises?.Count + subtopic.Constellations?.Count) ?? 1;

            var doneTopicTasksCount = 0;
            var subtopicsDto = new List<SubtopicDto>();
            
            foreach (var subtopic in subtopics)
            {
                var subtopicProgress = progress.Single(p => p.Subtopic.Id == subtopic.Id);
                doneTopicTasksCount += subtopicProgress.CompletedExercises;

                var count = subtopic.Exercises?.Count + subtopic.Constellations?.Count ?? 0;
                
                subtopicsDto.Add(new SubtopicDto
                {
                    Id = subtopic.Id,
                    Title = subtopic.Title,
                    Progress = count != 0 ?
                        subtopicProgress.CompletedExercises * 100 / count :
                        0,
                    State = subtopicProgress.IsOpen ? "IsOpen" : "IsLocked"
                });
            }
            
            topicsDto.Add(new TopicDto
            {
                Title = topic.Title,
                Progress = doneTopicTasksCount * 100 / topicTasksCount,
                Subtopics = subtopicsDto
            });
        }
        
        return topicsDto;
    }
}
