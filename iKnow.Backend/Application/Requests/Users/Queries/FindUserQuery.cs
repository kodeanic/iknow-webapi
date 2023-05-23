using System.ComponentModel.DataAnnotations;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Queries;

public class FindUserQuery : IRequest<UserDto>
{
    [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Invalid phone number")]
    public string Phone { get; }

    public FindUserQuery(string phone)
    {
        Phone = phone;
    }
}

public class FindUserQueryHandler : IRequestHandler<FindUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;

    public FindUserQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<UserDto> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        var phone = string.Join("", request.Phone.Where(char.IsDigit));

        var user = await _context.Users
            .Where(u => u.Phone == phone)
            .FirstOrDefaultAsync(cancellationToken);

        if(user is null)
            throw new NotFoundException("Пользователя не существует");

        return new UserDto
        {
            UserId = user.Id,
            PictureId = user.PictureId,
            Nickname = user.Nickname,
            Phone = user.Phone
        };
    }
}
