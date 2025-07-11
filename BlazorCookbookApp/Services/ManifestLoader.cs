using System.Text.Json;

namespace BlazorCookbookApp.Services;

/// <summary>
/// Loads recipe manifests from JSON files with fallback handling.
/// </summary>
public class ManifestLoader : IManifestLoader
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ManifestLoader> _logger;
    private readonly IConfiguration _configuration;
    
    // Cache the manifest to avoid repeated file I/O
    private RecipeManifest? _cachedManifest;
    private bool _loadAttempted = false;

    public ManifestLoader(IWebHostEnvironment environment, ILogger<ManifestLoader> logger, IConfiguration configuration)
    {
        _environment = environment;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Checks if manifest loading is available and configured.
    /// </summary>
    public bool IsManifestAvailable()
    {
        var manifestPath = GetManifestPath();
        return !string.IsNullOrEmpty(manifestPath) && File.Exists(manifestPath);
    }

    /// <summary>
    /// Loads recipe manifest from the configured JSON file.
    /// Returns null if manifest is not available or fails to load.
    /// </summary>
    public async Task<RecipeManifest?> LoadManifestAsync()
    {
        // Return cached manifest if already loaded
        if (_loadAttempted)
        {
            return _cachedManifest;
        }

        _loadAttempted = true;

        try
        {
            var manifestPath = GetManifestPath();
            if (string.IsNullOrEmpty(manifestPath))
            {
                _logger.LogInformation("No manifest path configured, using file scanning fallback");
                return null;
            }

            if (!File.Exists(manifestPath))
            {
                _logger.LogInformation("Manifest file not found at {ManifestPath}, using file scanning fallback", manifestPath);
                return null;
            }

            _logger.LogInformation("Loading recipe manifest from {ManifestPath}", manifestPath);

            var jsonContent = await File.ReadAllTextAsync(manifestPath);
            var manifest = JsonSerializer.Deserialize<RecipeManifest>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (manifest?.Recipes == null || manifest.Recipes.Count == 0)
            {
                _logger.LogWarning("Manifest loaded but contains no recipes, using file scanning fallback");
                return null;
            }

            _cachedManifest = manifest;
            _logger.LogInformation("Successfully loaded manifest with {RecipeCount} recipes (generated: {GeneratedAt})", 
                manifest.Recipes.Count, manifest.Metadata.GeneratedAt);

            return manifest;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse JSON manifest, using file scanning fallback");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error loading manifest, using file scanning fallback");
        }

        return null;
    }

    /// <summary>
    /// Gets the manifest file path from configuration or uses default locations.
    /// </summary>
    private string GetManifestPath()
    {
        // Check configuration for custom manifest path
        var configPath = _configuration["RecipeManifest:Path"];
        if (!string.IsNullOrEmpty(configPath))
        {
            return Path.IsPathRooted(configPath) ? configPath : Path.Combine(_environment.ContentRootPath, configPath);
        }

        // Default manifest locations
        var defaultPaths = new[]
        {
            Path.Combine(_environment.ContentRootPath, "recipe-manifest.json"),
            Path.Combine(_environment.ContentRootPath, "..", "recipe-manifest.json"),
            Path.Combine(_environment.WebRootPath, "recipe-manifest.json")
        };

        return defaultPaths.FirstOrDefault(File.Exists) ?? string.Empty;
    }
} 