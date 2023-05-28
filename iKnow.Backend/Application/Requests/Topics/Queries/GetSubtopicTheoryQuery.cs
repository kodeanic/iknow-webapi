using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Topics.Queries;

public class GetSubtopicTheoryQuery : IRequest<TheoryDto>
{
    public int SubtopicId { get; set; }
}

public class GetSubtopicTheoryQueryHandler : IRequestHandler<GetSubtopicTheoryQuery, TheoryDto>
{
    private readonly IApplicationDbContext _context;

    public GetSubtopicTheoryQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<TheoryDto> Handle(GetSubtopicTheoryQuery request, CancellationToken cancellationToken)
    {
        var subtopic = await _context.Subtopics
                         .Where(x => x.Id == request.SubtopicId)
                         .SingleOrDefaultAsync(cancellationToken) ??
                     throw new NotFoundException("Такой подтемы не существует");

        if (subtopic.Theory is null)
            throw new NotFoundException("Теории по этой теме не существует");

        return new TheoryDto
        {
            Theory = subtopic.Theory
        };
    }
}
