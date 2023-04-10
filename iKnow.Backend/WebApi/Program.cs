using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application;
using WebApi.Extensions;
using WebApi.Middlewares.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddCors();

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

var app = builder.Build();

app.Environment.EnvironmentName = "Development";
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
