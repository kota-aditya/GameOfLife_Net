using GameOfLife.Repositories;
using GameOfLife.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var storagePath = builder.Configuration.GetValue<string>("StoragePath");
if (string.IsNullOrEmpty(storagePath))
{
    storagePath = "Data";
}

storagePath = Path.Combine(builder.Environment.ContentRootPath, storagePath);

builder.Services.AddSingleton<IGameOfLifeRepository>(new FileGameOfLifeRepository(storagePath));
builder.Services.AddSingleton<IGameOfLifeService, GameOfLifeService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
