using RecipeManifestGenerator;

Console.WriteLine("🔍 BlazorCookbook Recipe Manifest Generator");
Console.WriteLine("==========================================");

// Get the solution root directory (assuming we're running from Tools/RecipeManifestGenerator)
var currentDirectory = Directory.GetCurrentDirectory();
var solutionRoot = Path.GetFullPath(Path.Combine(currentDirectory, "..", ".."));

Console.WriteLine($"📁 Solution root: {solutionRoot}");
Console.WriteLine($"🔍 Scanning for recipes...");

try
{
    var generator = new ManifestGenerator(solutionRoot);
    
    // Generate complete manifest
    Console.WriteLine("📋 Generating complete manifest...");
    var manifest = await generator.GenerateManifestAsync();
    
    Console.WriteLine($"\n✅ Manifest generated successfully!");
    Console.WriteLine($"📊 Generation timestamp: {manifest.Metadata.GeneratedAt:yyyy-MM-dd HH:mm:ss} UTC");
    Console.WriteLine($"📦 Generator version: {manifest.Metadata.GeneratorVersion}");
    Console.WriteLine($"📄 Format version: {manifest.Metadata.FormatVersion}");
    Console.WriteLine($"📁 Source path: {manifest.Metadata.SourcePath}");
    Console.WriteLine($"🗂️ Scanned directories: {manifest.Metadata.ScannedDirectories.Count}");
    
    // Display statistics
    Console.WriteLine("\n📊 Recipe Statistics:");
    Console.WriteLine($"  Total recipes: {manifest.Statistics.TotalRecipes}");
    Console.WriteLine($"  Visible recipes: {manifest.Statistics.VisibleRecipes}");
    Console.WriteLine($"  Hidden recipes: {manifest.Statistics.HiddenRecipes}");
    Console.WriteLine($"  Server recipes: {manifest.Statistics.ServerRecipes}");
    Console.WriteLine($"  Client recipes: {manifest.Statistics.ClientRecipes}");
    Console.WriteLine($"  Featured recipes (4+ stars): {manifest.Statistics.FeaturedRecipes}");
    Console.WriteLine($"  Chapter range: {manifest.Statistics.ChapterRange}");
    
    // Display star ratings distribution
    Console.WriteLine("\n⭐ Star Ratings Distribution:");
    foreach (var rating in manifest.Statistics.StarRatings.OrderBy(r => r.Key))
    {
        var stars = new string('★', rating.Key) + new string('☆', 5 - rating.Key);
        Console.WriteLine($"  {stars} ({rating.Key} stars): {rating.Value} recipes");
    }
    
    // Display visible recipes
    var visibleRecipes = manifest.Recipes.Where(r => r.VisibleInOverview).ToList();
    if (visibleRecipes.Any())
    {
        Console.WriteLine("\n🌟 Visible Recipes:");
        foreach (var recipe in visibleRecipes)
        {
            var stars = new string('★', recipe.Stars) + new string('☆', 5 - recipe.Stars);
            Console.WriteLine($"  {recipe.Route} - {recipe.Title} ({recipe.Location}) {stars}");
        }
    }
    
    // Display hidden recipes
    var hiddenRecipes = manifest.Recipes.Where(r => !r.VisibleInOverview).ToList();
    if (hiddenRecipes.Any())
    {
        Console.WriteLine("\n🔒 Hidden Recipes:");
        foreach (var recipe in hiddenRecipes)
        {
            var stars = new string('★', recipe.Stars) + new string('☆', 5 - recipe.Stars);
            Console.WriteLine($"  {recipe.Route} - {recipe.Title} ({recipe.Location}) {stars}");
        }
    }
    
    // Save to JSON file
    Console.WriteLine("\n💾 Saving manifest to JSON file...");
    var outputPath = Path.Combine(solutionRoot, "recipe-manifest.json");
    var savedPath = await generator.GenerateManifestFileAsync(outputPath);
    
    Console.WriteLine($"✅ Manifest saved to: {savedPath}");
    Console.WriteLine($"📄 File size: {new FileInfo(savedPath).Length:N0} bytes");
    
    Console.WriteLine("\n🎉 Recipe manifest generation completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}
