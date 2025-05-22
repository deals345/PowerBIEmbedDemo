using PowerBIEmbedDemo.Models;
using PowerBIEmbedDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

// Add HTTP client factory
services.AddHttpClient();

// Add HTTP context accessor
services.AddHttpContextAccessor();

// Configure PowerBISettings from appsettings.json
services.Configure<PowerBISettings>(configuration.GetSection("PowerBI"));

// Register PowerBIService
services.AddScoped<PowerBIService>();

// Add Razor Pages and MVC services
services.AddRazorPages();
services.AddControllersWithViews();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

// Fallback to the main page
app.MapFallbackToPage("/Index");

app.Run();
