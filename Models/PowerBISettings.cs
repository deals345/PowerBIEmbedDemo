namespace PowerBIEmbedDemo.Models;

public class PowerBISettings
{
    public string ReportId { get; set; } = string.Empty;
    public string EmbedUrl { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public bool UseAutoAuth { get; set; } = false;
    public bool IsPublicEmbed { get; set; } = false;
}
