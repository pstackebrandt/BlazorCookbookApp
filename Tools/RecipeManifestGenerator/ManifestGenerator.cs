using System.Text.RegularExpressions;
using System.Text.Json;

namespace RecipeManifestGenerator;

/// <summary>
/// Generates recipe manifests by scanning Blazor components for recipe pages following the /chXXrXX route pattern.
/// Automatically extracts chapter, recipe number, location, title, and summary information.
/// This is a console application version of the RecipeScanner service.
/// </summary>
public class ManifestGenerator
{
    private readonly string _rootPath;
    private readonly List<string> _scannedDirectories = new();
    
    // Matches @page "/ch01r02" or @page "/ch01r03cl" - captures route, chapter, recipe, variant
    private readonly Regex _routePattern = new(@"@page\s+""(/ch(\d+)r(\d+)(\w*))""", RegexOptions.IgnoreCase);
    
    // Extract PageTitle property (both public and protected override patterns)
    private readonly Regex _pageTitlePattern = new(@"(?:public\s+string\s+PageTitle\s*\{\s*get;\s*set;\s*\}\s*=\s*""([^""]*)""|protected\s+override\s+string\s+PageTitle\s*=>\s*""([^""]*)""\s*;)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    
    // Extract PageSummary property (both public and protected override patterns)
    private readonly Regex _pageSummaryPattern = new(@"(?:public\s+string\s+PageSummary\s*\{\s*get;\s*set;\s*\}\s*=\s*""([^""]*)""|protected\s+override\s+string\s+PageSummary\s*=>\s*""([^""]*)""\s*;)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    
    // Extract PageStars property (both public and protected override patterns)
    private readonly Regex _pageStarsPattern = new(@"(?:public\s+int\s+PageStars\s*\{\s*get;\s*set;\s*\}\s*=\s*(\d+)|protected\s+override\s+int\s+PageStars\s*=>\s*(\d+)\s*;)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    
    // Extract PageVisibleInOverview property (private static readonly pattern)
    private readonly Regex _pageVisibleInOverviewPattern = new(@"private\s+static\s+readonly\s+bool\s+PageVisibleInOverview\s*=\s*(true|false)\s*;", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    public ManifestGenerator(string rootPath)
    {
        _rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
    }

    /// <summary>
    /// Generates a complete recipe manifest with metadata and statistics.
    /// </summary>
    public async Task<RecipeManifest> GenerateManifestAsync()
    {
        var recipes = await GetRecipesAsync();
        var statistics = GenerateStatistics(recipes);
        var metadata = GenerateMetadata();
        
        return new RecipeManifest
        {
            Metadata = metadata,
            Recipes = recipes,
            Statistics = statistics
        };
    }

    /// <summary>
    /// Generates a complete recipe manifest and saves it to a JSON file.
    /// </summary>
    public async Task<string> GenerateManifestFileAsync(string outputPath)
    {
        var manifest = await GenerateManifestAsync();
        var json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        await File.WriteAllTextAsync(outputPath, json);
        return outputPath;
    }

    /// <summary>
    /// Scans all configured directories for recipe .razor files and returns organized recipe list.
    /// </summary>
    public async Task<List<RecipeInfo>> GetRecipesAsync()
    {
        var recipes = new List<RecipeInfo>();
        _scannedDirectories.Clear();

        // Scan server components (BlazorCookbookApp/Components)
        var serverPath = Path.Combine(_rootPath, "BlazorCookbookApp", "Components");
        if (Directory.Exists(serverPath))
        {
            await ScanDirectoryAsync(serverPath, "Server", recipes);
            _scannedDirectories.Add(serverPath);
        }

        // Scan client pages (BlazorCookbookApp.Client/Pages)
        var clientPagesPath = Path.Combine(_rootPath, "BlazorCookbookApp.Client", "Pages");
        if (Directory.Exists(clientPagesPath))
        {
            await ScanDirectoryAsync(clientPagesPath, "Client", recipes);
            _scannedDirectories.Add(clientPagesPath);
        }

        // Scan client chapters (BlazorCookbookApp.Client/Chapters)
        var clientChaptersPath = Path.Combine(_rootPath, "BlazorCookbookApp.Client", "Chapters");
        if (Directory.Exists(clientChaptersPath))
        {
            await ScanDirectoryAsync(clientChaptersPath, "Client", recipes);
            _scannedDirectories.Add(clientChaptersPath);
        }

        // Return sorted by chapter, recipe number, then variant
        return recipes
            .OrderBy(r => r.Chapter)
            .ThenBy(r => r.Recipe)
            .ThenBy(r => r.Variant)
            .ToList();
    }

    /// <summary>
    /// Generates comprehensive statistics about the recipes.
    /// </summary>
    private ManifestStatistics GenerateStatistics(List<RecipeInfo> recipes)
    {
        var visibleRecipes = recipes.Where(r => r.VisibleInOverview).ToList();
        var hiddenRecipes = recipes.Where(r => !r.VisibleInOverview).ToList();
        var serverRecipes = recipes.Where(r => r.Location == "Server").ToList();
        var clientRecipes = recipes.Where(r => r.Location == "Client").ToList();
        var featuredRecipes = recipes.Where(r => r.Stars >= 4).ToList();
        
        var chapters = recipes.Select(r => r.Chapter).ToList();
        var chapterRange = chapters.Any() ? $"{chapters.Min()}-{chapters.Max()}" : "0-0";
        
        var starRatings = recipes.GroupBy(r => r.Stars)
            .ToDictionary(g => g.Key, g => g.Count());

        return new ManifestStatistics
        {
            TotalRecipes = recipes.Count,
            VisibleRecipes = visibleRecipes.Count,
            HiddenRecipes = hiddenRecipes.Count,
            ServerRecipes = serverRecipes.Count,
            ClientRecipes = clientRecipes.Count,
            FeaturedRecipes = featuredRecipes.Count,
            ChapterRange = chapterRange,
            StarRatings = starRatings
        };
    }

    /// <summary>
    /// Generates metadata about the manifest generation process.
    /// </summary>
    private ManifestMetadata GenerateMetadata()
    {
        return new ManifestMetadata
        {
            GeneratedAt = DateTime.UtcNow,
            GeneratorVersion = "1.0.0",
            FormatVersion = "1.0",
            SourcePath = _rootPath,
            ScannedDirectories = _scannedDirectories.ToList()
        };
    }

    /// <summary>
    /// Recursively scans directory for .razor files and extracts recipe information.
    /// </summary>
    private async Task ScanDirectoryAsync(string directoryPath, string location, List<RecipeInfo> recipes)
    {
        try
        {
            var razorFiles = Directory.GetFiles(directoryPath, "*.razor", SearchOption.AllDirectories);

            foreach (var filePath in razorFiles)
            {
                try
                {
                    var content = await File.ReadAllTextAsync(filePath);
                    var recipe = ParseRecipeFile(content, filePath, location);
                    if (recipe != null)
                    {
                        recipes.Add(recipe);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue processing other files
                    Console.WriteLine($"Error parsing recipe file: {filePath} - {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scanning directory: {directoryPath} - {ex.Message}");
        }
    }

    /// <summary>
    /// Parses a .razor file content for recipe route pattern and extracts metadata.
    /// Returns null if no valid recipe route found.
    /// </summary>
    private RecipeInfo? ParseRecipeFile(string content, string filePath, string location)
    {
        // Match @page "/ch01r02" pattern
        var routeMatch = _routePattern.Match(content);
        if (!routeMatch.Success)
            return null;

        var route = routeMatch.Groups[1].Value;        // /ch01r02
        var chapterStr = routeMatch.Groups[2].Value;   // 01
        var recipeStr = routeMatch.Groups[3].Value;    // 02
        var variant = routeMatch.Groups[4].Value;      // cl (or empty)

        if (!int.TryParse(chapterStr, out var chapter) || !int.TryParse(recipeStr, out var recipe))
            return null;

        var title = ExtractPageTitle(content);
        var summary = ExtractPageSummary(content);
        var stars = ExtractPageStars(content);
        var visibleInOverview = ExtractPageVisibleInOverview(content);
        var filename = Path.GetFileNameWithoutExtension(filePath);

        return new RecipeInfo
        {
            Route = route,
            Chapter = chapter,
            Recipe = recipe,
            Variant = string.IsNullOrEmpty(variant) ? null : variant,
            Location = location,
            Title = title,
            Summary = summary,
            Stars = stars,
            VisibleInOverview = visibleInOverview,
            FilePath = filename
        };
    }

    /// <summary>
    /// Extracts recipe title from PageTitle property.
    /// Returns "unknown" if property not found.
    /// </summary>
    private string ExtractPageTitle(string content)
    {
        var match = _pageTitlePattern.Match(content);
        if (match.Success)
        {
            // Check both capture groups (public property and protected override)
            var title = match.Groups[1].Value.Trim();
            if (string.IsNullOrEmpty(title))
            {
                title = match.Groups[2].Value.Trim();
            }
            return title;
        }

        return "unknown";
    }

    /// <summary>
    /// Extracts recipe summary from PageSummary property.
    /// Returns "unknown" if property not found.
    /// </summary>
    private string ExtractPageSummary(string content)
    {
        var match = _pageSummaryPattern.Match(content);
        if (match.Success)
        {
            // Check both capture groups (public property and protected override)
            var summary = match.Groups[1].Value.Trim();
            if (string.IsNullOrEmpty(summary))
            {
                summary = match.Groups[2].Value.Trim();
            }
            return summary;
        }

        return "unknown";
    }

    /// <summary>
    /// Extracts recipe star rating from PageStars property.
    /// Returns 3 (default) if property not found.
    /// </summary>
    private int ExtractPageStars(string content)
    {
        var match = _pageStarsPattern.Match(content);
        if (match.Success)
        {
            // Check both capture groups (public property and protected override)
            var starsStr = match.Groups[1].Value.Trim();
            if (string.IsNullOrEmpty(starsStr))
            {
                starsStr = match.Groups[2].Value.Trim();
            }
            
            if (int.TryParse(starsStr, out var stars) && stars >= 1 && stars <= 5)
            {
                return stars;
            }
        }

        return 3; // Default 3 stars if not found or invalid
    }

    /// <summary>
    /// Extracts recipe visibility setting from PageVisibleInOverview property.
    /// Returns true (default) if property not found.
    /// </summary>
    private bool ExtractPageVisibleInOverview(string content)
    {
        var match = _pageVisibleInOverviewPattern.Match(content);
        if (match.Success)
        {
            var visibleStr = match.Groups[1].Value.Trim().ToLower();
            return visibleStr == "true";
        }

        return true; // Default to visible if not found
    }
} 