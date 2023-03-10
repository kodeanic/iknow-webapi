using Application.Requests.Users.Queries.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries.GetUser;

public class GetUserQuery : IRequest<UserDto>
{
    public int Id { get; set; }

    public GetUserQuery(int id)
    {
        Id = id;
    }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .Where(x => x.Id == request.Id)
            .Select(x => new UserDto()
            {
                Id = x.Id,
                Login = x.Login,
                Password = x.Password
            })
            .FirstOrDefaultAsync(cancellationToken);

        /*
        if (entity == null)
        {
            throw new NotFoundException(nameof(BicycleBrand), request.Id.ToString());
        }
        */
        return entity;
    }
}
