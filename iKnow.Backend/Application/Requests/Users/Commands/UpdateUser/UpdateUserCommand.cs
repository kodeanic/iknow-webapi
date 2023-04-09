using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using SwaggerIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Application.Requests.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    [SwaggerIgnore]
    public int Id { get; set; }

    [Required]
    public string LoginData { get; set; }

    [Required]
    public string Password { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        
        if (entity == null)
        {
            throw new Exception();
        }

        entity.LoginData = request.LoginData;
        entity.PasswordHash = HashPassword(request.Password);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private string HashPassword(string password)
    {
        var sha = SHA256.Create();

        var asByteArray = Encoding.UTF8.GetBytes(password);
        var hashedPassword = sha.ComputeHash(asByteArray);

        return Convert.ToBase64String(hashedPassword);
    }
}
