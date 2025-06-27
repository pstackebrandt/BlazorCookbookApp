using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorCookbookApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register services
builder.Services.AddScoped<IRecipeUrlService, RecipeUrlService>();

await builder.Build().RunAsync();
