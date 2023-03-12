using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Application.Requests.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<User>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public LoginUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(LoginUserCommand request, CancellationToken _)
    {
        var entity = await _context.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();

        if(entity == null)
            throw new Exception(message: "Такого юзера не существует");

        if (entity.PasswordHash == HashPassword(request.Password))
            return entity;
        else
            throw new Exception(message: "Неверный пароль!!!");
    }

    private string HashPassword(string password)
    {
        var sha = SHA256.Create();

        var asByteArray = Encoding.UTF8.GetBytes(password);
        var hashedPassword = sha.ComputeHash(asByteArray);

        return Convert.ToBase64String(hashedPassword);
    }
}
