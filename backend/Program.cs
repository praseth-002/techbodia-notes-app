using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var storageProvider = builder.Configuration["StorageProvider"] ?? "InMemory";
var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:5173", "http://127.0.0.1:5173"];

if (string.Equals(storageProvider, "SqlServer", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<INoteRepository, NoteRepository>();
}
else
{
    builder.Services.AddSingleton<INoteRepository, InMemoryNoteRepository>();
}

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

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();