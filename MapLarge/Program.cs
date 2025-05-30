// Basic configuration and middleware pipeline for serving the API and SPA frontend

// Configure the application builder and dependency injection

var builder = WebApplication.CreateBuilder(args);

// Register controller support
builder.Services.AddControllers();

builder.Services.Configure<FileBrowserOptions>(builder.Configuration.GetSection("FileBrowser"));

// Enable CORS for cross-origin frontend access
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.WebHost.UseUrls("http://*:8080");

var app = builder.Build();

// Use CORS and serve static files from wwwroot/index.html
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

// Map API controllers (e.g., /api/browse)
app.MapControllers();

app.Run();