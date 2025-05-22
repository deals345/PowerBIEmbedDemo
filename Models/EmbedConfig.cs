namespace PowerBIEmbedDemo.Models
{
    public class EmbedConfig
    {
        public string Type { get; set; } = "report";
        public string EmbedUrl { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public object Settings { get; set; } = new
        {
            panes = new
            {
                filters = new { expanded = false, visible = false },
                pageNavigation = new { visible = true }
            },
            background = "transparent"
        };
    }
}
