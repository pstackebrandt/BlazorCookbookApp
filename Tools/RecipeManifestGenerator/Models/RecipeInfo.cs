using System.Text.Json.Serialization;

namespace RecipeManifestGenerator;

/// <summary>
/// Represents metadata for a recipe found in the Blazor Cookbook application.
/// Contains route information, location, title, and summary extracted from .razor files.
/// This is a console application version of the BlazorCookbookApp.Services.RecipeInfo class.
/// </summary>
public class RecipeInfo
{
    /// <summary>Complete route path (e.g., "/ch01r02")</summary>
    [JsonPropertyName("route")]
    public string Route { get; set; } = string.Empty;
    
    /// <summary>Chapter number (e.g., 1)</summary>
    [JsonPropertyName("chapter")]
    public int Chapter { get; set; }
    
    /// <summary>Recipe number within chapter (e.g., 2)</summary>
    [JsonPropertyName("recipe")]
    public int Recipe { get; set; }
    
    /// <summary>Optional variant suffix (e.g., "cl" for client)</summary>
    [JsonPropertyName("variant")]
    public string? Variant { get; set; }
    
    /// <summary>Project location: "Client" or "Server"</summary>
    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;
    
    /// <summary>Recipe title extracted from PageTitle property</summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>Recipe description extracted from PageSummary property</summary>
    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;
    
    /// <summary>Star rating for the recipe (1-5 stars, where 4+ stars are featured)</summary>
    [JsonPropertyName("stars")]
    public int Stars { get; set; } = 3;
    
    /// <summary>Whether the recipe is visible in the Browse Recipes overview (default: true)</summary>
    [JsonPropertyName("visibleInOverview")]
    public bool VisibleInOverview { get; set; } = true;
    
    /// <summary>Full file path to the .razor file</summary>
    [JsonPropertyName("filePath")]
    public string FilePath { get; set; } = string.Empty;
} 