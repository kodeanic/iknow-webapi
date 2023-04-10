using System.ComponentModel.DataAnnotations;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries;

public class FindUserQuery : IRequest<User>
{
    [Required]
    [Phone]
    public string Phone { get; }

    public FindUserQuery(string phone)
    {
        Phone = phone;
    }
}

public class FindUserQueryHandler : IRequestHandler<FindUserQuery, User>
{
    private readonly IApplicationDbContext _context;

    public FindUserQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(u => u.Phone == request.Phone)
            .FirstOrDefaultAsync(cancellationToken);

        return user ?? throw new NotFoundException("Пользователя не существует");
    }
}
