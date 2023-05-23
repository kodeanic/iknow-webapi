using Application.Common.Dto;
using Application.Common.Exceptions;
using Domain.Entities.Constellations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Constellations.Queries;

public class GetConstellationQuery : IRequest<ConstellationDto>
{
    public int Id { get; set; }

    public GetConstellationQuery(int id)
    {
        Id = id;
    }
}

public class GetConstellationQueryHandler : IRequestHandler<GetConstellationQuery, ConstellationDto>
{
    private readonly IApplicationDbContext _context;

    public GetConstellationQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<ConstellationDto> Handle(GetConstellationQuery request, CancellationToken cancellationToken)
    {
        var constellation = await _context.Constellations
            .Include(c => c.Lines)
            .Include(c => c.Stars)
            .Where(c => c.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (constellation is null)
            throw new NotFoundException("Такого созвездия не существует");

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
        
        return response;
    }
}
