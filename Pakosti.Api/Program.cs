using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IPakostiDbContext).Assembly));
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<PakostiDbContext>();
await context.Database.MigrateAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();