using System.ComponentModel.DataAnnotations;

namespace PowerBIEmbedDemo.Models;

public class PowerBISettings
{
    [Required]
    public string ReportId { get; set; } = string.Empty;

    [Required]
    [Url]
    public string EmbedUrl { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty; // GroupId remains optional

    [Required]
    public string TenantId { get; set; } = string.Empty;
    public bool UseAutoAuth { get; set; } = false;
    public bool IsPublicEmbed { get; set; } = false;
}
