using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries.FindUser;

public class FindUserQuery : IRequest<User>
{
    public string Email{ get; set; }

    public FindUserQuery(string email)
    {
        Email = email;
    }
}

public class FindUserQueryHandler : IRequestHandler<FindUserQuery, User>
{
    private readonly IApplicationDbContext _context;

    public FindUserQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        return entity;
    }
}
