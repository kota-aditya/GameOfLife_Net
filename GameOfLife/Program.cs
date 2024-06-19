using GameOfLife.Repositories;
using GameOfLife.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var storagePath = builder.Configuration.GetValue<string>("StoragePath") ?? "Data";
storagePath = Path.Combine(builder.Environment.ContentRootPath, storagePath);

builder.Services.AddSingleton<IGameOfLifeRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<FileGameOfLifeRepository>>();
    return new FileGameOfLifeRepository(storagePath, logger);
});

builder.Services.AddSingleton<IGameOfLifeService, GameOfLifeService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Conway's Game of Life API",
        Description = "An API to manage and simulate Conway's Game of Life boards."
    });
    options.EnableAnnotations(); // Enable annotations
});
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conway's Game of Life API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
