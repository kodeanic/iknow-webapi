using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Subjects.Queries;

public class GetTopicsQuery : IRequest<List<Topic>>
{
    public string Subject { get; set; }
}

public class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, List<Topic>>
{
    private readonly IApplicationDbContext _context;

    public GetTopicsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<Topic>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
    {
        if(!Enum.TryParse(request.Subject, out SubjectList type))
            throw new BadRequestException("Неправильно указан предмет");
        
        var subject = await _context.Subjects
            .Include(s => s.Topics)
            .ThenInclude(t => t.Subtopics)
            .Where(s => s.SubjectType == type)
            .SingleOrDefaultAsync(cancellationToken);
        
        return subject?.Topics ?? throw new NotFoundException("Предмет не найден");
    }
}
