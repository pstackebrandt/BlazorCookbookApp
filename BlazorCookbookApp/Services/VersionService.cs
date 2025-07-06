using System.Reflection;

namespace BlazorCookbookApp.Services;

/// <summary>
/// Service for retrieving application version information
/// </summary>
public interface IVersionService
{
    /// <summary>
    /// Gets the formatted version string from assembly metadata
    /// </summary>
    string GetVersion();
}

/// <summary>
/// Implementation that reads version from executing assembly
/// </summary>
public class VersionService : IVersionService
{
    public string GetVersion()
    {
        // Get version from assembly metadata (defined in Directory.Build.props)
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        if (version != null)
        {
            // Return formatted version with 3 parts (Major.Minor.Build)
            return $"Version {version.Major}.{version.Minor}.{version.Build}";
        }
        return "Version unknown";
    }
} 