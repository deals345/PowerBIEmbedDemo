using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using PowerBIEmbedDemo.Models;
using PowerBIEmbedDemo.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

// Add HTTP client factory
services.AddHttpClient();

// Add authentication with Microsoft Identity Web
services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// Add Microsoft Identity consent handler
services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

// Add Microsoft Identity UI
services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Add HTTP context accessor
services.AddHttpContextAccessor();

// Add session state for storing tokens
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Register PowerBIService
services.AddScoped<PowerBIService>();

// Configure PowerBISettings from appsettings.json
services.Configure<PowerBISettings>(configuration.GetSection("PowerBI"));

// Add application services
services.AddRazorPages();
services.AddControllersWithViews();

// Add HTTP client factory
services.AddHttpClient();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add distributed memory cache (used by the token cache)
services.AddDistributedMemoryCache();

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

// Fallback to the main page
app.MapFallbackToPage("/Index");

app.Run();
