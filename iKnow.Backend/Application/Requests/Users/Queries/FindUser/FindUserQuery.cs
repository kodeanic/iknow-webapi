using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries.FindUser;

public class FindUserQuery : IRequest<bool>
{
    public string Email{ get; set; }

    public FindUserQuery(string email)
    {
        Email = email;
    }
}

public class FindUserQueryHandler : IRequestHandler<FindUserQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public FindUserQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        return entity != null;
    }
}
