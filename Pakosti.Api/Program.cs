using Pakosti.Infrastructure.Persistence;
using Pakosti.Infrastructure.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddHostedService<DataStoreInitializer>();

var app = builder.Build();


app.MapControllers();

app.Run();