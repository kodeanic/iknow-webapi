using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Exceptions;

namespace Application.Requests.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<User>
{
    [Required]
    public string LoginData { get; set; }

    [Required]
    public string Password { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public LoginUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.Where(u => u.LoginData == request.LoginData).FirstOrDefaultAsync(cancellationToken);

        if(entity == null)
            throw new BadRequestException(message: "Такого юзера не существует");

        return entity.PasswordHash == HashPassword(request.Password) ? entity : throw new BadRequestException(message: "Неверный пароль!!!");
    }

    private string HashPassword(string password)
    {
        var sha = SHA256.Create();

        var asByteArray = Encoding.UTF8.GetBytes(password);
        var hashedPassword = sha.ComputeHash(asByteArray);

        return Convert.ToBase64String(hashedPassword);
    }
}
