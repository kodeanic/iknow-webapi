using Application.Common.Exceptions;
using Domain.Entities.Constellations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Constellations.Queries;

public class GetConstellationQuery : IRequest<Constellation>
{
    public int Id { get; set; }

    public GetConstellationQuery(int id)
    {
        Id = id;
    }
}

public class GetConstellationQueryHandler : IRequestHandler<GetConstellationQuery, Constellation>
{
    private readonly IApplicationDbContext _context;

    public GetConstellationQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<Constellation> Handle(GetConstellationQuery request, CancellationToken cancellationToken)
    {
        var constellation = await _context.Constellations
            .Include(c => c.Lines)
            .Include(c => c.Stars)
            .Where(c => c.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (constellation is null)
            throw new NotFoundException("Такого созвездия не существует");
        
        return constellation;
    }
}
