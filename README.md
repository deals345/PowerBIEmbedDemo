# Power BI Embed Demo

This is a .NET 6.0 application that demonstrates how to embed a Power BI dashboard in a web application using Microsoft Identity for authentication.

## Prerequisites

- .NET 6.0 SDK or later
- A Power BI Pro or Premium account
- An Azure AD application with the necessary API permissions

## Configuration

1. Clone the repository
2. Update the `appsettings.json` file with your Azure AD and Power BI settings:
   ```json
   {
     "AzureAd": {
       "Instance": "https://login.microsoftonline.com/",
       "Domain": "yourdomain.onmicrosoft.com",
       "TenantId": "your-tenant-id",
       "ClientId": "your-client-id",
       "CallbackPath": "/signin-oidc"
     },
     "PowerBI": {
       "ApiUrl": "https://api.powerbi.com/",
       "GroupId": "your-workspace-id",
       "DashboardId": "your-dashboard-id"
     }
   }
   ```

3. Add your client secret to the `appsettings.Development.json` file (this file is in .gitignore):
   ```json
   {
     "AzureAd": {
       "ClientSecret": "your-client-secret"
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

3. Open a web browser and navigate to `https://localhost:7059` or `http://localhost:5037`

## Features

- Secure authentication with Azure AD
- Power BI dashboard embedding
- Responsive layout
- Error handling and logging

## Project Structure

- `Pages/` - Contains the Razor pages
- `Services/` - Contains the Power BI service implementation
- `wwwroot/` - Contains static files (CSS, JavaScript, etc.)
- `appsettings.json` - Configuration file
- `Program.cs` - Application startup configuration

## Dependencies

- Microsoft.Identity.Web
- Microsoft.PowerBI.Api
- Microsoft.Identity.Client
