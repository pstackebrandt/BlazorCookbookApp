using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace BlazorCookbookApp.Client.Services;

/// <summary>
/// Service for extracting chapter and recipe numbers from recipe page URLs
/// and generating formatted titles and text.
/// </summary>
public interface IRecipeUrlService
{
    /// <summary>
    /// Initializes the service by parsing the current URL to extract 
    /// chapter and recipe numbers.
    /// </summary>
    /// <param name="navigationManager">The NavigationManager to get current URL from</param>
    void Initialize(NavigationManager navigationManager);
    
    /// <summary>
    /// Gets the chapter number extracted from the URL (e.g., 1 from "/ch01r04").
    /// Returns 0 if no valid chapter number found.
    /// </summary>
    int ChapterNumber { get; }
    
    /// <summary>
    /// Gets the recipe number extracted from the URL (e.g., 4 from "/ch01r04").
    /// Returns 0 if no valid recipe number found.
    /// </summary>
    int RecipeNumber { get; }
    
    /// <summary>
    /// Returns a formatted string like "Chapter 1, Recipe 4" based on 
    /// extracted URL parameters, or "Unknown Chapter/Recipe" if extraction failed.
    /// </summary>
    string GetFormattedChapterRecipe();
    
    /// <summary>
    /// Combines a base title with the formatted chapter/recipe information.
    /// </summary>
    /// <param name="baseTitle">The developer-defined page title</param>
    /// <returns>Formatted title like "Render modes (Chapter 1, Recipe 4)"</returns>
    string GetTitleWithNumbers(string baseTitle);
}

/// <summary>
/// Implementation of IRecipeUrlService that extracts chapter and recipe numbers
/// from URLs matching the pattern "/ch##r##" (e.g., "/ch01r04").
/// </summary>
public class RecipeUrlService : IRecipeUrlService
{
    /// <summary>
    /// Regex pattern to match recipe URLs like "/ch01r04" and extract numbers.
    /// Group 1: Chapter number, Group 2: Recipe number
    /// </summary>
    private readonly Regex _routePattern = new(@"/ch(\d+)r(\d+)", RegexOptions.IgnoreCase);
    
    /// <inheritdoc/>
    public int ChapterNumber { get; private set; }
    
    /// <inheritdoc/>
    public int RecipeNumber { get; private set; }

    /// <inheritdoc/>
    public void Initialize(NavigationManager navigationManager)
    {
        ExtractRouteNumbers(navigationManager);
    }

    /// <inheritdoc/>
    public string GetFormattedChapterRecipe()
    {
        if (ChapterNumber > 0 && RecipeNumber > 0)
        {
            return $"Chapter {ChapterNumber}, Recipe {RecipeNumber}";
        }
        return "Unknown Chapter/Recipe";
    }

    /// <inheritdoc/>
    public string GetTitleWithNumbers(string baseTitle)
    {
        return $"{baseTitle} ({GetFormattedChapterRecipe()})";
    }

    /// <summary>
    /// Extracts chapter and recipe numbers from the current URL path.
    /// Uses regex to match "/ch##r##" pattern and parses the numeric groups.
    /// </summary>
    /// <param name="navigationManager">NavigationManager to get current URL</param>
    private void ExtractRouteNumbers(NavigationManager navigationManager)
    {
        // Get the absolute URI and extract the path portion
        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
        var path = uri.AbsolutePath;
        
        // Try to match the recipe URL pattern (e.g., "/ch01r04")
        var match = _routePattern.Match(path);
        if (match.Success)
        {
            // Extract and parse chapter number (first captured group)
            ChapterNumber = int.Parse(match.Groups[1].Value);
            // Extract and parse recipe number (second captured group)
            RecipeNumber = int.Parse(match.Groups[2].Value);
        }
        // If no match found, ChapterNumber and RecipeNumber remain 0
    }
} 