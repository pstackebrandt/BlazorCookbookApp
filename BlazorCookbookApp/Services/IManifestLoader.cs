namespace BlazorCookbookApp.Services;

/// <summary>
/// Interface for loading recipe manifests from various sources (JSON files, etc.)
/// </summary>
public interface IManifestLoader
{
    /// <summary>
    /// Loads recipe manifest from the configured source.
    /// Returns null if manifest is not available or fails to load.
    /// </summary>
    /// <returns>Recipe manifest or null if not available</returns>
    Task<RecipeManifest?> LoadManifestAsync();
    
    /// <summary>
    /// Checks if manifest loading is available and configured.
    /// </summary>
    /// <returns>True if manifest loading is available</returns>
    bool IsManifestAvailable();
} 