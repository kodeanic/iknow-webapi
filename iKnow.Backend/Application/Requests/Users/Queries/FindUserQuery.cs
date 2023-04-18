using System.ComponentModel.DataAnnotations;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries;

public class FindUserQuery : IRequest<User>
{
    [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Invalid phone number")]
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
        var phone = string.Join("", request.Phone.Where(char.IsDigit));

        var user = await _context.Users
            .Where(u => u.Phone == phone)
            .FirstOrDefaultAsync(cancellationToken);

        return user ?? throw new NotFoundException("Пользователя не существует");
    }
}
