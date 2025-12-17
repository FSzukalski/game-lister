using GameLister.Api.Data;
using GameLister.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS – na czas dev pozwalamy na wszystko
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddScoped<IListingPreviewService, ListingPreviewService>();
builder.Services.AddScoped<IAllegroOfferPayloadService, AllegroOfferPayloadService>(); builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); // wbudowane OpenAPI z .NET 9

// ---- EF Core + SQLite ----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=game-lister.db";

builder.Services.AddDbContext<GameListerDbContext>(options =>
    options.UseSqlite(connectionString));
// ---------------------------

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // endpoint z dokumentem OpenAPI
}

app.UseHttpsRedirection();
app.UseCors();
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
