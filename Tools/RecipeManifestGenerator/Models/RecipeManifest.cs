using System.Text.Json.Serialization;

namespace RecipeManifestGenerator;

/// <summary>
/// Represents a complete recipe manifest with metadata and recipe list.
/// This manifest is generated at build time and consumed by the web application.
/// </summary>
public class RecipeManifest
{
    /// <summary>Manifest metadata information</summary>
    [JsonPropertyName("metadata")]
    public ManifestMetadata Metadata { get; set; } = new();
    
    /// <summary>List of all recipes found during scanning</summary>
    [JsonPropertyName("recipes")]
    public List<RecipeInfo> Recipes { get; set; } = new();
    
    /// <summary>Statistics about the recipes in this manifest</summary>
    [JsonPropertyName("statistics")]
    public ManifestStatistics Statistics { get; set; } = new();
}

/// <summary>
/// Metadata about when and how the manifest was generated.
/// </summary>
public class ManifestMetadata
{
    /// <summary>When the manifest was generated (UTC)</summary>
    [JsonPropertyName("generatedAt")]
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>Version of the manifest generator that created this file</summary>
    [JsonPropertyName("generatorVersion")]
    public string GeneratorVersion { get; set; } = "1.0.0";
    
    /// <summary>Format version of the manifest structure</summary>
    [JsonPropertyName("formatVersion")]
    public string FormatVersion { get; set; } = "1.0";
    
    /// <summary>Path where the manifest was generated from</summary>
    [JsonPropertyName("sourcePath")]
    public string SourcePath { get; set; } = string.Empty;
    
    /// <summary>Directories that were scanned for recipes</summary>
    [JsonPropertyName("scannedDirectories")]
    public List<string> ScannedDirectories { get; set; } = new();
}

/// <summary>
/// Statistics about the recipes in the manifest.
/// </summary>
public class ManifestStatistics
{
    /// <summary>Total number of recipes found</summary>
    [JsonPropertyName("totalRecipes")]
    public int TotalRecipes { get; set; }
    
    /// <summary>Number of visible recipes</summary>
    [JsonPropertyName("visibleRecipes")]
    public int VisibleRecipes { get; set; }
    
    /// <summary>Number of hidden recipes</summary>
    [JsonPropertyName("hiddenRecipes")]
    public int HiddenRecipes { get; set; }
    
    /// <summary>Number of recipes in Server project</summary>
    [JsonPropertyName("serverRecipes")]
    public int ServerRecipes { get; set; }
    
    /// <summary>Number of recipes in Client project</summary>
    [JsonPropertyName("clientRecipes")]
    public int ClientRecipes { get; set; }
    
    /// <summary>Number of featured recipes (4+ stars)</summary>
    [JsonPropertyName("featuredRecipes")]
    public int FeaturedRecipes { get; set; }
    
    /// <summary>Chapter range (e.g., "1-99")</summary>
    [JsonPropertyName("chapterRange")]
    public string ChapterRange { get; set; } = string.Empty;
    
    /// <summary>Recipe count by star rating</summary>
    [JsonPropertyName("starRatings")]
    public Dictionary<int, int> StarRatings { get; set; } = new();
} 