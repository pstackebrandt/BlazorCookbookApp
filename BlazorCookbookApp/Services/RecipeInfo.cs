namespace BlazorCookbookApp.Services;

/// <summary>
/// Represents metadata for a recipe found in the Blazor Cookbook application.
/// Contains route information, location, title, and summary extracted from .razor files.
/// </summary>
public class RecipeInfo
{
    /// <summary>Complete route path (e.g., "/ch01r02")</summary>
    public string Route { get; set; } = string.Empty;
    
    /// <summary>Chapter number (e.g., 1)</summary>
    public int Chapter { get; set; }
    
    /// <summary>Recipe number within chapter (e.g., 2)</summary>
    public int Recipe { get; set; }
    
    /// <summary>Optional variant suffix (e.g., "cl" for client)</summary>
    public string? Variant { get; set; }
    
    /// <summary>Project location: "Client" or "Server"</summary>
    public string Location { get; set; } = string.Empty;
    
    /// <summary>Recipe title extracted from PageTitle property</summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>Recipe description extracted from PageSummary property</summary>
    public string Summary { get; set; } = string.Empty;
    
    /// <summary>Star rating for the recipe (1-5 stars, where 4+ stars are featured)</summary>
    public int Stars { get; set; } = 3;
    
    /// <summary>Whether the recipe is visible in the Browse Recipes overview (default: true)</summary>
    public bool VisibleInOverview { get; set; } = true;
    
    /// <summary>Full file path to the .razor file</summary>
    public string FilePath { get; set; } = string.Empty;
} 