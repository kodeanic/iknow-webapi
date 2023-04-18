using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries;

public class GetProgressQuery : IRequest<List<Progress>>
{
    public User User { get; set; }

    public GetProgressQuery(User user)
    {
        User = user;
    }
}

public class GetTopicsQueryHandler : IRequestHandler<GetProgressQuery, List<Progress>>
{
    private readonly IApplicationDbContext _context;

    public GetTopicsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<Progress>> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var progress = await _context.Progresses
            .Include(p => p.User)
            .Include(p => p.Topic)
            .Where(p => p.User.Id == request.User.Id)
            .ToListAsync(cancellationToken);
        
        return progress;
    }
}
