﻿using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Exceptions;

namespace Application.Requests.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<User>
{
    [Required]
    public string LoginData { get; set; }

    [Required]
    public string Password { get; set; }
    
    public string? Nickname { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.LoginData == request.LoginData, cancellationToken);

        var user = entity is not null ? throw new BadRequestException("Пользователь уже существует") : new User()
        {
            LoginData = request.LoginData,
            PasswordHash = HashPassword(request.Password),
            Nickname = request.Nickname
        };

        await _context.Users.AddAsync(user, cancellationToken);
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
