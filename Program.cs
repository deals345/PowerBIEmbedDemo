using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using PowerBIEmbedDemo.Models;
using PowerBIEmbedDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

// Add HTTP client factory
services.AddHttpClient();

// Add Microsoft Identity Web authentication
services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));

// Add Microsoft Identity UI and token acquisition
services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Add Microsoft Identity Web token acquisition
services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureAd")
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// Add HTTP context accessor
services.AddHttpContextAccessor();

// Add session state for storing tokens
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register PowerBIService
services.AddScoped<PowerBIService>();

// Configure PowerBISettings from appsettings.json
services.Configure<PowerBISettings>(configuration.GetSection("PowerBI"));

// Add session state for storing tokens
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add application services
services.AddHttpContextAccessor();
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Add HttpsRedirection and StaticFiles middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware
app.UseSession();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapRazorPages();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
