using System.ComponentModel.DataAnnotations;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Commands;

public class DeleteUserCommand : IRequest
{
    [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Invalid phone number")]
    public string Phone { get; }

    public DeleteUserCommand(string phone)
    {
        Phone = phone;
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var phone = string.Join("", request.Phone.Where(char.IsDigit));
        
        var user = await _context.Users
            .Where(u => u.Phone == phone)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
        {
            throw new NotFoundException("Пользователя не существует");
        }

        var tokens = await _context.UserRefreshTokens
            .Where(t => t.User.Id == user.Id)
            .ToListAsync(cancellationToken);
        
        var progress = await _context.Progresses
            .Include(p => p.User)
            .Where(p => p.User.Id == user.Id)
            .ToListAsync(cancellationToken);
        
        _context.Progresses.RemoveRange(progress);
        _context.UserRefreshTokens.RemoveRange(tokens);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
