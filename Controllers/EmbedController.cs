using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerBIEmbedDemo.Models;
using PowerBIEmbedDemo.Services;

namespace PowerBIEmbedDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmbedController : ControllerBase
    {
        private readonly IOptions<PowerBISettings> _powerBISettings;
        private readonly ILogger<EmbedController> _logger;

        public EmbedController(IOptions<PowerBISettings> powerBISettings, ILogger<EmbedController> logger)
        {
            _powerBISettings = powerBISettings;
            _logger = logger;
        }

        [HttpGet("config")]
        public IActionResult GetEmbedConfig()
        {
            try
            {
                var settings = _powerBISettings.Value;
                
                // Log the settings being used
                _logger.LogInformation($"Using Power BI settings - ReportId: {settings.ReportId}, GroupId: {settings.GroupId}");
                
                // Construct the embed URL with the report ID and group ID
                var embedUrl = $"{settings.EmbedUrl}?reportId={settings.ReportId}&groupId={settings.GroupId}";
                
                var embedConfig = new EmbedConfig
                {
                    Type = "report",
                    EmbedUrl = embedUrl,
                    ReportId = settings.ReportId,
                    GroupId = settings.GroupId,
                    Settings = new 
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
                return Ok(embedConfig);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating embed configuration");
                return StatusCode(500, new { error = "Failed to generate embed configuration", details = ex.Message });
            }
        }
    }
}
