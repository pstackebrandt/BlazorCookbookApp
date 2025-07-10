# Health Check Implementation Guide

## Overview

This guide documents the implementation of a basic health check endpoint for
.NET 9 Blazor projects deployed to Azure App Service.

### Why We Need This

- **Azure App Service Monitoring**: Automatic health monitoring and failure detection
- **Load Balancer Health Checks**: Ensures traffic routes only to healthy instances
- **Deployment Validation**: Simple endpoint to verify successful deployments
- **Application Insights**: Automatic tracking and metrics collection
- **Production Standard**: Industry best practice for web applications

## Implementation

### 1. Code Changes

Add to `Program.cs`:

```csharp
// Add after existing service registrations
builder.Services.AddHealthChecks();

// Add before app.Run()
app.MapHealthChecks("/health");
```

### 2. Complete Example

```csharp
// Program.cs
using BlazorExamples2.Client.Pages;
using BlazorExamples2.Components;
using BlazorExamples2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Register your services
builder.Services.AddScoped<IVersionService, VersionService>();

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorExamples2.Client._Imports).Assembly);

// Map health check endpoint
app.MapHealthChecks("/health");

app.Run();
```

## Testing

### Local Testing

1. **Run the application**:

   ```powershell
   dotnet run --urls "http://localhost:5000"
   ```

2. **Test the endpoint**:

   ```powershell
   Invoke-WebRequest -Uri "http://localhost:5000/health" -Method GET
   ```

3. **Expected Response**:
   - **Status**: 200 OK
   - **Content**: "Healthy"

### Azure Testing

After deployment, test:

```powershell
Invoke-WebRequest -Uri "https://yourapp.azurewebsites.net/health" -Method GET
```

## Azure App Service Integration

### Automatic Features

- **Health Check Detection**: Azure automatically detects the `/health` endpoint
- **Application Insights**: Endpoint calls are automatically tracked
- **Monitoring**: Built-in health monitoring capabilities

### Optional Configuration

To explicitly configure health checks in Azure App Service:

1. Go to your **App Service** → **Health check**
2. Enable health check monitoring
3. Set path to `/health`
4. Configure failure threshold (default: 3 failures)

### Production URL

Your health check will be available at:

```text
https://yourapp.azurewebsites.net/health
```

## Troubleshooting

### Common Issues

1. **404 Not Found**: Ensure `app.MapHealthChecks("/health")` is called before `app.Run()`
2. **HTTPS Redirect**: Health checks work with both HTTP and HTTPS
3. **Build Errors**: Ensure you're using .NET 9 (health checks are built-in)

### Verification Steps

1. Check build succeeds: `dotnet build`
2. Test locally before deploying
3. Verify Azure deployment includes the changes
4. Test production endpoint after deployment

## Notes

- **No Authentication**: Health checks bypass authentication by design
- **Minimal Logging**: Health check calls don't clutter application logs
- **Performance**: Negligible impact on application performance
- **Standards**: Follows ASP.NET Core and Azure best practices

## Implementation Date

December 2024 - Based on .NET 9 GA support on Azure App Service
