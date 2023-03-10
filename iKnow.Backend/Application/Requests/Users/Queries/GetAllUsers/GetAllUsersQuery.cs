using Application.Requests.Users.Queries.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<IList<UserDto>> { }

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IList<UserDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<IList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Users
            .Select(x => new UserDto()
            {
                Id = x.Id,
                Login = x.Login,
                Password = x.Password
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}
