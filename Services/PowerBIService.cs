using Microsoft.Extensions.Options;
using PowerBIEmbedDemo.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Encodings.Web;
using System.Web;

namespace PowerBIEmbedDemo.Services;

public class PowerBIService
{
    private readonly PowerBISettings _settings;
    private readonly ILogger<PowerBIService> _logger;

    public PowerBIService(IOptions<PowerBISettings> settings, ILogger<PowerBIService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public string GetEmbedConfig()
    {
        // Build the embed URL with all required parameters
        var uriBuilder = new UriBuilder(_settings.EmbedUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        query["reportId"] = _settings.ReportId;
        query["autoAuth"] = "true";

        if (!string.IsNullOrEmpty(_settings.TenantId))
        {
            query["ctid"] = _settings.TenantId;
        }

        if (!string.IsNullOrEmpty(_settings.GroupId) && _settings.GroupId.ToLower() != "me")
        {
            query["groupId"] = _settings.GroupId;
        }

        uriBuilder.Query = query.ToString();
        var embedUrl = uriBuilder.ToString();
        
        var config = new
        {
            type = "report",
            embedUrl = embedUrl,
            tokenType = 5, // 5 = AAD token, 1 = Embed for your organization
            accessToken = string.Empty, // This should be a valid token in production
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
        
        _logger.LogInformation($"Generated embed configuration for ReportId: '{_settings.ReportId}', GroupId: '{_settings.GroupId}'. Embed URL: {embedUrl}");
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
