# Power BI Embed Demo

This is a .NET 6.0 application that demonstrates how to embed a Power BI report in a web application using direct iframe embedding with auto-authentication.

## Prerequisites

- .NET 6.0 SDK or later
- A Power BI Pro or Premium account
- Access to the Power BI report you want to embed
- The report must be published to the web or shared with appropriate permissions

## Configuration

1. Clone the repository
2. Update the `appsettings.json` file with your Power BI settings:
   ```json
   {
     "PowerBI": {
       "ReportId": "your-report-id",
       "EmbedUrl": "https://app.powerbi.com/reportEmbed",
       "ApiUrl": "https://api.powerbi.com/",
       "GroupId": "me",  // Use "me" for personal workspace or the workspace ID
       "TenantId": "your-tenant-id",
       "IsPublicEmbed": true
     }
   }
   ```

## Running the Application

1. Restore the NuGet packages:
   ```bash
   dotnet restore
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open a web browser and navigate to:
   - `https://localhost:7059` (HTTPS)
   - `http://localhost:5037` (HTTP)

## Features

- Direct iframe embedding of Power BI reports
- Auto-authentication with Power BI
- Responsive layout that works on all device sizes
- Simple configuration with appsettings.json
- Error handling and user feedback

## Project Structure

- `Pages/` - Contains the Razor pages
- `Services/` - Contains the Power BI service implementation
- `Controllers/` - Contains the API controllers
- `Models/` - Contains the data models
- `wwwroot/` - Contains static files (CSS, JavaScript, etc.)
- `appsettings.json` - Configuration file
- `Program.cs` - Application startup configuration

## Implementation Details

This application uses direct iframe embedding with the following URL format:
```
https://app.powerbi.com/reportEmbed?reportId={ReportId}&autoAuth=true&ctid={TenantId}
```

### Key Components

1. **Index.cshtml**: Contains the iframe that embeds the Power BI report
2. **PowerBIService.cs**: Handles the generation of the embed URL
3. **EmbedController.cs**: Provides API endpoints for client-side embedding
4. **appsettings.json**: Stores configuration values for the Power BI report

## Security Notes

- The implementation uses auto-authentication, which requires the user to be signed in to Power BI
- For production use, consider implementing proper authentication and authorization
- Ensure that the Power BI report has the appropriate sharing settings configured

## Troubleshooting

If you encounter issues:
1. Check the browser's developer console (F12) for any error messages
2. Verify that the report ID and tenant ID in appsettings.json are correct
3. Ensure you have the necessary permissions to access the report
4. Make sure the report is published to the web or shared with your account

## Dependencies

- .NET 6.0 Runtime
- ASP.NET Core Runtime 6.0
- Power BI JavaScript client library (loaded from CDN)
