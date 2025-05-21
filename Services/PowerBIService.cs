using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Microsoft.Extensions.Options;
using PowerBIEmbedDemo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PowerBIEmbedDemo.Services;

public class PowerBIService
{
    private readonly PowerBISettings _settings;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly ILogger<PowerBIService> _logger;

    public PowerBIService(IOptions<PowerBISettings> settings, ITokenAcquisition tokenAcquisition, ILogger<PowerBIService> logger)
    {
        _settings = settings.Value;
        _tokenAcquisition = tokenAcquisition;
        _logger = logger;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        try
        {
            // Get token for Power BI API
            var token = await _tokenAcquisition.GetAccessTokenForAppAsync("https://analysis.windows.net/.default");
            return token;
        }
        catch (MicrosoftIdentityWebChallengeUserException ex)
        {
            _logger.LogError(ex, "Authentication challenge failed");
            throw new Exception("Authentication challenge failed. Please sign in again.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error acquiring access token");
            throw new Exception("Error acquiring access token", ex);
        }
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
    
    public string? GetDashboardId()
    {
        return _settings.DashboardId;
    }

    public string? GetGroupId()
    {
        return _settings.GroupId;
    }
}
