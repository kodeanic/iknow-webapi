using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Commands;

public class UpdateUserCommand : IRequest<User>
{
    public int UserId { get; set; }
    
    public int PictureId { get; set; }
    
    public string? Nickname { get; set; }
    
    [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Invalid phone number")]
    public string Phone { get; set; }
    
    public string Password { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(u => u.Id == request.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException("Пользователь не найден");

        user.PictureId = request.PictureId;
        if (request.Nickname is not null)
            user.Nickname = request.Nickname;
        user.Phone = string.Join("", request.Phone.Where(char.IsDigit));
        user.PasswordHash = HashPassword(request.Password);

        await _context.SaveChangesAsync(cancellationToken);
        
        return user;
    }

    private string HashPassword(string password)
    {
        var sha = SHA256.Create();

        var asByteArray = Encoding.UTF8.GetBytes(password);
        var hashedPassword = sha.ComputeHash(asByteArray);

        return Convert.ToBase64String(hashedPassword);
    }
}