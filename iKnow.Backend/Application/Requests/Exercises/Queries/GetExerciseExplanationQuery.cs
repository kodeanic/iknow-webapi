using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Exercises.Queries;

public class GetExerciseExplanationQuery : IRequest<ExplanationDto>
{
    public int ExerciseId { get; set; }
}

public class GetExerciseExplanationQueryHandler : IRequestHandler<GetExerciseExplanationQuery, ExplanationDto>
{
    private readonly IApplicationDbContext _context;

    public GetExerciseExplanationQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<ExplanationDto> Handle(GetExerciseExplanationQuery request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises
                           .Where(x => x.Id == request.ExerciseId)
                           .SingleOrDefaultAsync(cancellationToken) ??
                       throw new NotFoundException("Такого задания не существует");
        
        if (exercise.Explanation is null)
            throw new NotFoundException("Объяснения по этой задаче нет");

        return new ExplanationDto
        {
            Explanation = exercise.Explanation
        };
    }
}
