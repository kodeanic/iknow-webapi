using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Exceptions;

namespace Application.Requests.Users.Commands;

public class LoginUserCommand : IRequest<User>
{
    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    public string Password { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public LoginUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(u => u.Phone == request.Phone).FirstOrDefaultAsync(cancellationToken);

        if(user == null)
            throw new NotFoundException("Неверный номер телефона");

        return user.PasswordHash != HashPassword(request.Password) ?
            throw new BadRequestException("Неверный пароль") :
            user;
    }

    private string HashPassword(string password)
    {
        var sha = SHA256.Create();

        var asByteArray = Encoding.UTF8.GetBytes(password);
        var hashedPassword = sha.ComputeHash(asByteArray);

        return Convert.ToBase64String(hashedPassword);
    }
}
