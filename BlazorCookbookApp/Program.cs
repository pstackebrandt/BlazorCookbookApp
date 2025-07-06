using BlazorCookbookApp.Client.Pages;
using BlazorCookbookApp.Components;
using BlazorCookbookApp.Services;
using BlazorCookbookApp.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add recipe scanner service
builder.Services.AddScoped<RecipeScanner>();

// Add recipe URL service (needed for InteractiveWebAssembly pages that pre-render on server)
builder.Services.AddScoped<IRecipeUrlService, RecipeUrlService>();

// Add version service
builder.Services.AddScoped<IVersionService, VersionService>();

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

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorCookbookApp.Client._Imports).Assembly);

app.Run();
