using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries.FindUser;

public class FindUserQuery : IRequest<User>
{
    public string? LoginData { get; }

    public FindUserQuery(string loginData)
    {
        LoginData = loginData;
    }
}

public class FindUserQueryHandler : IRequestHandler<FindUserQuery, User>
{
    private readonly IApplicationDbContext _context;

    public FindUserQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        var entity = request.LoginData is null ? throw new BadRequestException() :
            await _context.Users
            .Where(x => x.LoginData == request.LoginData)
            .FirstOrDefaultAsync(cancellationToken);

        return entity ?? throw new NotFoundException("Такого пользователя не существует");
    }
}
