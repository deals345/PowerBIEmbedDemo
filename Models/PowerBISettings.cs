namespace PowerBIEmbedDemo.Models;

public class PowerBISettings
{
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AuthorityUrl { get; set; } = string.Empty;
    public string ResourceUrl { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public string DashboardId { get; set; } = string.Empty;
}
