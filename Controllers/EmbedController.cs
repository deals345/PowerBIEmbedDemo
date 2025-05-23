using Microsoft.AspNetCore.Mvc;
using PowerBIEmbedDemo.Services;
using Microsoft.Extensions.Logging;
using System;

namespace PowerBIEmbedDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmbedController : ControllerBase
    {
        private readonly PowerBIService _powerBIService;
        private readonly ILogger<EmbedController> _logger;

        public EmbedController(PowerBIService powerBIService, ILogger<EmbedController> logger)
        {
            _powerBIService = powerBIService;
            _logger = logger;
        }

        [HttpGet("config")]
        public IActionResult GetEmbedConfig()
        {
            try
            {
                // Log that we are about to get the config from the service
                _logger.LogInformation("Attempting to retrieve embed configuration from PowerBIService.");

                string jsonConfig = _powerBIService.GetEmbedConfig();
                
                // Log successful retrieval
                _logger.LogInformation("Successfully retrieved embed configuration from PowerBIService.");
                return Content(jsonConfig, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating embed configuration");
                return StatusCode(500, new { error = "Failed to generate embed configuration", details = ex.Message });
            }
        }
    }
}
