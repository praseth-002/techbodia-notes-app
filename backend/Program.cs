using backend.Data;
using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var storageProvider = builder.Configuration["StorageProvider"]?.Trim() ?? "InMemory";
var useMySql = string.Equals(storageProvider, "MySql", StringComparison.OrdinalIgnoreCase);

if (useMySql)
{
    builder.Services.AddSingleton<MySqlConnectionFactory>();
    builder.Services.AddSingleton<MySqlDatabaseInitializer>();
    builder.Services.AddScoped<INoteRepository, MySqlNoteRepository>();
}
else if (string.Equals(storageProvider, "InMemory", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddSingleton<INoteRepository, InMemoryNoteRepository>();
}
else
{
    throw new InvalidOperationException($"Unsupported StorageProvider '{storageProvider}'. Use 'InMemory' or 'MySql'.");
}

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:5173", "http://127.0.0.1:5173"];

// CORS — allows your Vue frontend to talk to this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (useMySql)
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<MySqlDatabaseInitializer>();
    await initializer.EnsureDatabaseAsync();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();