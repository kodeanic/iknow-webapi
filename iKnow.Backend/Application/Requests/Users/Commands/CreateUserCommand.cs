using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Exceptions;

namespace Application.Requests.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Invalid phone number")]
    public string Phone { get; set; }
    
    public string Password { get; set; }
    
    public string? Nickname { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var phone = string.Join("", request.Phone.Where(char.IsDigit));
        
        var entity = await _context.Users.
            Where(e => e.Phone == phone).FirstOrDefaultAsync(cancellationToken);

        var user = entity is not null ?
            throw new BadRequestException("Пользователь с таким номером телефона уже существует") :
            new User
            {
                PictureId = 0,
                Phone = phone, 
                PasswordHash = HashPassword(request.Password), 
                Nickname = request.Nickname 
            };

        var topics = await _context.Topics
            .Include(t => t.Subtopics)
            .ToListAsync(cancellationToken);
        
        var progress = new List<Progress>();
        
        foreach (var topic in topics)
        {
            var subtopics = topic.Subtopics;
            foreach (var subtopic in subtopics)
            {
                progress.Add(new Progress
                {
                    User = user,
                    Subtopic = subtopic,
                    IsOpen = subtopics.First() == subtopic
                });
            }
        }
        
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.Progresses.AddRangeAsync(progress, cancellationToken);
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
