using BlazorCookbookApp.Client.Pages;
using BlazorCookbookApp.Components;
using BlazorCookbookApp.Services;
using BlazorCookbookApp.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add recipe scanner service with JSON manifest loading capability
builder.Services.AddScoped<IManifestLoader, ManifestLoader>();
builder.Services.AddScoped<RecipeScanner>();

// Add recipe URL service (needed for InteractiveWebAssembly pages that pre-render on server)
builder.Services.AddScoped<IRecipeUrlService, RecipeUrlService>();

// Add version service
builder.Services.AddScoped<IVersionService, VersionService>();

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Azure handles HTTPS termination at load balancer level, avoiding redirect loops
// Behind the load balancer, the app is served over HTTP
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); // Local development needs explicit HTTPS redirection
}
// Production: Azure infrastructure handles HTTPS termination automatically
// Redirects in production could lead to redirect loops and connection issues.

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorCookbookApp.Client._Imports).Assembly);

// Map health check endpoint
app.MapHealthChecks("/health");

app.Run();
