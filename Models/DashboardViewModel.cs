namespace PowerBIEmbedDemo.Models;

public class DashboardViewModel
{
    /// <summary>
    /// The URL to embed the Power BI dashboard
    /// </summary>
    public string EmbedUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// The access token for embedding the dashboard
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// The JSON configuration for embedding the dashboard
    /// </summary>
    public string EmbedConfig { get; set; } = string.Empty;
    
    /// <summary>
    /// URL to the dashboard's thumbnail image
    /// </summary>
    public string ThumbnailUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// The dashboard ID
    /// </summary>
    public string DashboardId { get; set; } = string.Empty;
    
    /// <summary>
    /// The workspace (group) ID that contains the dashboard
    /// </summary>
    public string GroupId { get; set; } = string.Empty;
    
    /// <summary>
    /// Indicates if the dashboard is loaded successfully
    /// </summary>
    public bool IsLoaded { get; set; }
    
    /// <summary>
    /// Error message if dashboard loading fails
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
}
