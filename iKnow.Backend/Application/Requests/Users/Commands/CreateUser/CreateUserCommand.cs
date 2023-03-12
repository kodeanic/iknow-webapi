using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Application.Requests.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<User>
{
    [Required]
    public string Login { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken _)
    {
        var entity = await _context.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();

        if (entity != null)
            throw new Exception(message: "Такой юзер уже есть");

        entity = new User()
        {
            Login = request.Login,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password)
        };

        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    private string HashPassword(string password)
    {
        var sha = SHA256.Create();

        var asByteArray = Encoding.UTF8.GetBytes(password);
        var hashedPassword = sha.ComputeHash(asByteArray);

        return Convert.ToBase64String(hashedPassword);
    }
}
