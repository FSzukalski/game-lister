using GameLister.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); // wbudowane OpenAPI z .NET 9

// ---- EF Core + SQLite ----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=game-lister.db";

builder.Services.AddDbContext<GameListerDbContext>(options =>
    options.UseSqlite(connectionString));
// ---------------------------

var app = builder.Build();

// automatyczne utworzenie bazy (bez migracji, na start wystarczy)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GameListerDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // endpoint z dokumentem OpenAPI
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () =>
    Results.Json(new
    {
        status = "ok",
        message = "GameLister API is running",
        version = "1.0"
    }));
app.Run();
