using System.ComponentModel.DataAnnotations;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Commands;

public class DeleteUserCommand : IRequest
{
    [Required]
    [Phone]
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
        var user = await _context.Users
            .Where(u => u.Phone == request.Phone)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
        {
            throw new NotFoundException("Пользователя не существует");
        }
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
