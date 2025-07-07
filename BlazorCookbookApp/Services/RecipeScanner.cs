using System.Text.RegularExpressions;

namespace BlazorCookbookApp.Services;

/// <summary>
/// Scans Blazor components for recipe pages following the /chXXrXX route pattern.
/// Automatically extracts chapter, recipe number, location, title, and summary information.
/// </summary>
public class RecipeScanner
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<RecipeScanner> _logger;
    
    // Matches @page "/ch01r02" or @page "/ch01r03cl" - captures route, chapter, recipe, variant
    private readonly Regex _routePattern = new(@"@page\s+""(/ch(\d+)r(\d+)(\w*))""", RegexOptions.IgnoreCase);
    
    // Extract PageTitle property (both public and protected override patterns)
    private readonly Regex _pageTitlePattern = new(@"(?:public\s+string\s+PageTitle\s*\{\s*get;\s*set;\s*\}\s*=\s*""([^""]*)""|protected\s+override\s+string\s+PageTitle\s*=>\s*""([^""]*)""\s*;)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    
    // Extract PageSummary property (both public and protected override patterns)
    private readonly Regex _pageSummaryPattern = new(@"(?:public\s+string\s+PageSummary\s*\{\s*get;\s*set;\s*\}\s*=\s*""([^""]*)""|protected\s+override\s+string\s+PageSummary\s*=>\s*""([^""]*)""\s*;)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    
    // Extract PageStars property (both public and protected override patterns)
    private readonly Regex _pageStarsPattern = new(@"(?:public\s+int\s+PageStars\s*\{\s*get;\s*set;\s*\}\s*=\s*(\d+)|protected\s+override\s+int\s+PageStars\s*=>\s*(\d+)\s*;)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    public RecipeScanner(IWebHostEnvironment environment, ILogger<RecipeScanner> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    /// Scans all configured directories for recipe .razor files and returns organized recipe list.
    /// </summary>
    public async Task<List<RecipeInfo>> GetRecipesAsync()
    {
        var recipes = new List<RecipeInfo>();
        var webRoot = _environment.ContentRootPath;

        // Scan server components (BlazorCookbookApp/Components)
        var serverPath = Path.Combine(webRoot, "Components");
        if (Directory.Exists(serverPath))
        {
            await ScanDirectoryAsync(serverPath, "Server", recipes);
        }

        // Scan client pages (BlazorCookbookApp.Client/Pages)
        var clientPagesPath = Path.Combine(webRoot, "..", "BlazorCookbookApp.Client", "Pages");
        if (Directory.Exists(clientPagesPath))
        {
            await ScanDirectoryAsync(clientPagesPath, "Client", recipes);
        }

        // Scan client chapters (BlazorCookbookApp.Client/Chapters)
        var clientChaptersPath = Path.Combine(webRoot, "..", "BlazorCookbookApp.Client", "Chapters");
        if (Directory.Exists(clientChaptersPath))
        {
            await ScanDirectoryAsync(clientChaptersPath, "Client", recipes);
        }

        // Return sorted by chapter, recipe number, then variant
        return recipes
            .OrderBy(r => r.Chapter)
            .ThenBy(r => r.Recipe)
            .ThenBy(r => r.Variant)
            .ToList();
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
                    _logger.LogError(ex, "Error parsing recipe file: {FilePath}", filePath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning directory: {DirectoryPath}", directoryPath);
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
} 