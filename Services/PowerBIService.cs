using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using PowerBIEmbedDemo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PowerBIEmbedDemo.Services;

public class PowerBIService
{
    private readonly IOptions<PowerBISettings> _settings;
    private readonly IConfiguration _configuration;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PowerBIService> _logger;

    public PowerBIService(IOptions<PowerBISettings> settings, IConfiguration configuration, ITokenAcquisition tokenAcquisition, IHttpClientFactory httpClientFactory, ILogger<PowerBIService> logger)
    {
        _settings = settings;
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var scopes = new[] { $"{_settings.Value.ResourceUrl}/.default" };
        var result = await _tokenAcquisition.GetAccessTokenForClientAsync(scopes);
        return result.AccessToken;
    }

    public async Task<EmbedToken> GetDashboardEmbedTokenAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            
            // Create token credentials
            var tokenCredentials = new TokenCredentials(token, "Bearer");
            
            // Initialize Power BI client with token credentials
            var client = new PowerBIClient(new TokenCredentials(token, "Bearer"))
            {
                BaseUri = new Uri("https://api.powerbi.com")
            };
            
            var groupId = Guid.Parse(_settings.GroupId);
            var dashboardId = Guid.Parse(_settings.DashboardId);
            
            // Create token request parameters
            var tokenRequest = new GenerateTokenRequest(
                accessLevel: "View"
            );
            
            // Get the embed token
            var embedToken = await client.Dashboards.GenerateTokenInGroupAsync(
                groupId,
                dashboardId,
                tokenRequest);

            return embedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating dashboard embed token");
            throw;
        }
    }

    public async Task<string> GetDashboardThumbnailUrlAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            
            // Initialize Power BI client with token credentials
            var client = new PowerBIClient(new TokenCredentials(token, "Bearer"))
            {
                BaseUri = new Uri("https://api.powerbi.com")
            };
            
            var groupId = Guid.Parse(_settings.GroupId);
            var dashboardId = Guid.Parse(_settings.DashboardId);
            
            // Get the dashboard to get the embed URL
            var dashboard = await client.Dashboards.GetDashboardInGroupAsync(groupId, dashboardId);
            
            if (dashboard == null)
            {
                throw new Exception("Dashboard not found");
            }
            
            // Return the embed URL which can be used to show a thumbnail
            return $"https://app.powerbi.com/dashboards/{dashboard.Id}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard thumbnail URL");
            // Return a default thumbnail or null if the dashboard can't be accessed
            return null;
        }
    }

    public string GetEmbedConfig()
    {
        var config = new
        {
            type = "dashboard",
            tokenType = 1,
            embedUrl = $"https://app.powerbi.com/dashboardEmbed?dashboardId={_settings.DashboardId}&groupId={_settings.GroupId}",
            dashboardId = _settings.DashboardId,
            groupId = _settings.GroupId
        };
        
        return JsonSerializer.Serialize(config);
    }
    
    public string GetDashboardId()
    {
        return _settings.DashboardId;
    }
    
    public string GetGroupId()
    {
        return _settings.GroupId;
    }
}
