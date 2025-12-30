using GameLister.Api;
using GameLister.Api.Data;
using GameLister.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// -------------------- CORS (dev) --------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// -------------------- Controllers + OpenAPI --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// -------------------- Storage paths (outside repo) --------------------
// ContentRoot: ...\backend\GameLister.Api
// Repo root:   ...\ (2 levels up)
// Storage:     ...\.storage
var storageRoot = builder.Configuration["Storage:RootPath"];
if (string.IsNullOrWhiteSpace(storageRoot))
{
    storageRoot = Path.GetFullPath(Path.Combine(
        builder.Environment.ContentRootPath, "..", "..", ".storage"));
}

Directory.CreateDirectory(storageRoot);

var imagesRoot = Path.Combine(storageRoot, "images");
Directory.CreateDirectory(imagesRoot);

// Expose to DI
builder.Services.AddSingleton(new StoragePaths(storageRoot, imagesRoot));

// -------------------- EF Core + SQLite (DB outside repo) --------------------
var dbPath = builder.Configuration["Storage:DatabasePath"];
if (string.IsNullOrWhiteSpace(dbPath))
{
    dbPath = Path.Combine(storageRoot, "game-lister.db");
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? $"Data Source={dbPath}";

builder.Services.AddDbContext<GameListerDbContext>(options =>
    options.UseSqlite(connectionString));

// -------------------- Services --------------------
builder.Services.AddScoped<IListingPreviewService, ListingPreviewService>();
builder.Services.AddScoped<IAllegroOfferPayloadService, AllegroOfferPayloadService>();

var app = builder.Build();

// -------------------- Serve images as static files --------------------
// Public URLs:
//   /storage/images/<file>
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesRoot),
    RequestPath = "/storage/images"
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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


