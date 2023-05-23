using Domain.Entities;
using Domain.Entities.Constellations;
using Microsoft.EntityFrameworkCore;

namespace Application;

public interface IApplicationDbContext : IDisposable
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Subtopic> Subtopics { get; set; }
    
    public DbSet<Progress> Progresses { get; set; }
    
    public DbSet<Exercise> Exercises { get; set; }
    
    public DbSet<Constellation> Constellations { get; set; }
    public DbSet<Star> Stars { get; set; }
    public DbSet<Line> Lines { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}
