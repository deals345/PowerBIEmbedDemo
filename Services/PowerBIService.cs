using Microsoft.Extensions.Options;
using PowerBIEmbedDemo.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace PowerBIEmbedDemo.Services;

public class PowerBIService
{
    private readonly PowerBISettings _settings;
    private readonly ILogger<PowerBIService> _logger;
    private const string PublicEmbedToken = "EMBED_TOKEN"; // This is just a placeholder

    public PowerBIService(IOptions<PowerBISettings> settings, ILogger<PowerBIService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public string GetEmbedConfig()
    {
        // Build the embed URL with all required parameters
        var embedUrl = $"{_settings.EmbedUrl}?reportId={_settings.ReportId}";
        
        // Always add autoAuth=true and the tenant ID
        embedUrl += "&autoAuth=true";
        
        // Add the tenant ID (ctid) if provided
        if (!string.IsNullOrEmpty(_settings.TenantId))
        {
            embedUrl += $"&ctid={_settings.TenantId}";
        }
        
        // Add group ID only if it's not "me"
        if (!string.IsNullOrEmpty(_settings.GroupId) && _settings.GroupId.ToLower() != "me")
        {
            embedUrl += $"&groupId={_settings.GroupId}";
        }
        
        var config = new
        {
            type = "report",
            embedUrl = embedUrl,
            tokenType = 5, // 5 = AAD token, 1 = Embed for your organization
            accessToken = PublicEmbedToken, // This should be a valid token in production
            reportId = _settings.ReportId,
            groupId = _settings.GroupId,
            settings = new
            {
                panes = new
                {
                    filters = new { expanded = false, visible = false },
                    pageNavigation = new { visible = true }
                },
                background = "transparent",
                layoutType = "custom"
            }
        };
        
        _logger.LogInformation($"Generated embed URL: {embedUrl}");
        return JsonSerializer.Serialize(config);
    }
    
    public string? GetReportId()
    {
        return _settings.ReportId;
    }

    public string? GetGroupId()
    {
        return _settings.GroupId;
    }
    
    public string GetEmbedUrl()
    {
        return _settings.EmbedUrl;
    }
}
