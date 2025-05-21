using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PowerBIEmbedDemo.Models;
using PowerBIEmbedDemo.Services;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PowerBIEmbedDemo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly PowerBIService _powerBIService;
    private readonly IHttpClientFactory _httpClientFactory;

    public DashboardViewModel ViewModel { get; set; } = new();

    public IndexModel(ILogger<IndexModel> logger, PowerBIService powerBIService, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _powerBIService = powerBIService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            _logger.LogInformation("Starting to load Power BI dashboard...");
            
            // Get the dashboard details
            var dashboardId = _powerBIService.GetDashboardId();
            var groupId = _powerBIService.GetGroupId();
            var embedUrl = $"https://app.powerbi.com/dashboardEmbed?dashboardId={dashboardId}&groupId={groupId}";
            
            _logger.LogInformation("Getting embed token...");
            // Get the embed token
            var embedToken = await _powerBIService.GetDashboardEmbedTokenAsync();
            
            if (embedToken == null || string.IsNullOrEmpty(embedToken.Token))
            {
                _logger.LogError("Failed to get a valid embed token");
                throw new Exception("Failed to get a valid embed token from Power BI service");
            }
            
            _logger.LogInformation("Getting dashboard thumbnail URL...");
            // Get the thumbnail URL
            string thumbnailUrl = null;
            try 
            {
                thumbnailUrl = await _powerBIService.GetDashboardThumbnailUrlAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not load dashboard thumbnail");
                // Continue without the thumbnail if there's an error
            }
            
            // Create the view model
            var config = new 
            {
                type = "dashboard",
                tokenType = 1,
                accessToken = embedToken.Token,
                embedUrl = embedUrl,
                dashboardId = dashboardId,
                groupId = groupId
            };
            
            ViewModel = new DashboardViewModel
            {
                ThumbnailUrl = thumbnailUrl,
                EmbedConfig = JsonSerializer.Serialize(config, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                }),
                DashboardId = dashboardId,
                GroupId = groupId,
                EmbedUrl = embedUrl,
                Token = embedToken.Token,
                IsLoaded = true
            };

            _logger.LogInformation("Successfully loaded dashboard configuration");
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading Power BI dashboard");
            // Return the page even if there's an error, but log it
            ModelState.AddModelError(string.Empty, "Error loading Power BI dashboard. Please check the configuration.");
            return Page();
        }
    }
    
    public async Task<IActionResult> OnGetEmbedInfo()
    {
        try
        {
            var token = await _powerBIService.GetDashboardEmbedTokenAsync();
            var embedConfig = _powerBIService.GetEmbedConfig();
            
            if (token == null || string.IsNullOrEmpty(embedConfig))
            {
                return StatusCode(500, new { error = "Failed to generate embed token or configuration." });
            }
            
            return new JsonResult(new
            {
                token = token.Token,
                tokenId = token.TokenId,
                expiration = token.Expiration,
                embedUrl = $"https://app.powerbi.com/dashboardEmbed?dashboardId={_powerBIService.GetDashboardId()}",
                embedConfig = JsonSerializer.Deserialize<object>(embedConfig)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating embed info");
            return StatusCode(500, new { error = "An error occurred while generating the embed information." });
        }
    }
}
