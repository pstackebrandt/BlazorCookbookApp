# Version Implementation Guide

This guide shows how to implement comprehensive version management in a .NET project, including central version management, runtime version access, and UI display.

## Overview

This implementation provides:
- ✅ Central version management across all projects
- ✅ Runtime access to version information
- ✅ UI display of current version
- ✅ Centralized NuGet package management
- ✅ Semantic versioning support

## Step-by-Step Implementation

### Step 1: Create Directory.Build.props

Create `Directory.Build.props` in your **solution root** (same level as .sln file):

```xml
<Project>
  <PropertyGroup>
    <!-- Central Version Management -->
    <Version>1.0.0</Version>
    <AssemblyVersion>1.0.0.0</AssemblyVersion>
    <FileVersion>1.0.0.0</FileVersion>
    <PackageVersion>1.0.0</PackageVersion>
    
    <!-- Additional Metadata -->
    <Company>YourCompanyName</Company>
    <Product>YourProductName</Product>
    <Description>Your project description</Description>
    <Copyright>Copyright © $([System.DateTime]::Now.Year)</Copyright>
  </PropertyGroup>
</Project>
```

**What this does:**
- `Version`: General version identifier (3-part format)
- `AssemblyVersion`: .NET assembly version (4-part format)
- `FileVersion`: Windows file version (4-part format)
- `PackageVersion`: NuGet package version (3-part format)

### Step 2: Create Directory.Packages.props

Create `Directory.Packages.props` in your **solution root**:

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  
  <ItemGroup>
    <!-- Central Package Version Management -->
    <!-- Add your NuGet packages here -->
    <!-- Example: -->
    <!-- <PackageVersion Include="Microsoft.AspNetCore.Components.WebAssembly" Version="9.0.5" /> -->
  </ItemGroup>
</Project>
```

**What this does:**
- Enables central package version management
- All NuGet package versions managed in one place
- Prevents version conflicts across projects

### Step 3: Create Version Service

Create a new file `Services/VersionService.cs`:

```csharp
using System.Reflection;

namespace YourProject.Services;

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
```

**What this does:**
- Provides runtime access to version information
- Reads version from assembly metadata (set by Directory.Build.props)
- Returns formatted version string

### Step 4: Register Version Service

In your `Program.cs`, add the service registration:

```csharp
using YourProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Register version service
builder.Services.AddScoped<IVersionService, VersionService>();

var app = builder.Build();
```

**What this does:**
- Registers the version service for dependency injection
- Makes it available throughout your application

### Step 5: Display Version in UI

In your navigation component (e.g., `NavMenu.razor`):

```razor
@using YourProject.Services
@inject IVersionService VersionService

<!-- Your existing navigation content -->

<!-- Add version display at the bottom -->
<div class="version-info px-3 mt-auto pb-3">
    <small>@VersionService.GetVersion()</small>
</div>
```

**What this does:**
- Injects the version service
- Displays current version in the UI
- Usually shown in navigation or footer

### Step 6: Update Project Files (if needed)

If you have existing projects with PackageReference entries, update them:

**Before:**
```xml
<PackageReference Include="SomePackage" Version="1.0.0" />
```

**After:**
```xml
<PackageReference Include="SomePackage" />
```

The version will be managed centrally in `Directory.Packages.props`.

## How to Update Versions

### 1. Update Directory.Build.props

Edit version numbers in `Directory.Build.props`:

```xml
<!-- Example: Bug fix release -->
<Version>1.0.1</Version>
<AssemblyVersion>1.0.1.0</AssemblyVersion>
<FileVersion>1.0.1.0</FileVersion>
<PackageVersion>1.0.1</PackageVersion>
```

### 2. Update Package Versions

Add/update packages in `Directory.Packages.props`:

```xml
<PackageVersion Include="NewPackage" Version="2.1.0" />
```

### 3. Build and Test

```powershell
dotnet build
```

The new version will be embedded in all assemblies and available at runtime.

## Version Types Explained

| Type            | Format                  | Purpose                 | Example |
| --------------- | ----------------------- | ----------------------- | ------- |
| Version         | MAJOR.MINOR.PATCH       | General identification  | 1.2.0   |
| AssemblyVersion | MAJOR.MINOR.PATCH.BUILD | .NET runtime binding    | 1.2.0.0 |
| FileVersion     | MAJOR.MINOR.PATCH.BUILD | Windows file properties | 1.2.0.0 |
| PackageVersion  | MAJOR.MINOR.PATCH       | NuGet package version   | 1.2.0   |

## Semantic Versioning Guidelines

- **MAJOR**: Breaking changes (1.x.x → 2.0.0)
- **MINOR**: New features, backward-compatible (1.1.x → 1.2.0)
- **PATCH**: Bug fixes, minor improvements (1.1.0 → 1.1.1)

## Benefits of This Approach

1. **Central Management**: All versions in one place
2. **Consistency**: Same version across all projects
3. **Runtime Access**: Version available in application code
4. **UI Display**: Users can see current version
5. **Package Management**: Centralized NuGet dependencies
6. **Easy Updates**: Change version in one file

## Troubleshooting

### Version Not Updating
- Clean and rebuild: `dotnet clean && dotnet build`
- Check Directory.Build.props is in solution root
- Verify file is not corrupted

### Service Not Found
- Ensure service is registered in Program.cs
- Check namespace imports
- Verify using statement in components

### Package Conflicts
- Use `dotnet list package --outdated` to check versions
- Ensure all projects use centralized package management

## File Structure

```
YourSolution/
├── Directory.Build.props          # Central version management
├── Directory.Packages.props       # Central package management
├── YourSolution.sln
└── YourProject/
    ├── Services/
    │   └── VersionService.cs      # Version service implementation
    ├── Program.cs                 # Service registration
    └── Components/
        └── Layout/
            └── NavMenu.razor      # Version display
```

## Example Commands

```powershell
# Check current version after build
dotnet build

# Update all packages
dotnet list package --outdated

# Create git tag for version
git tag -a v1.2.0 -m "Version 1.2.0 release"
git push origin v1.2.0
```

This implementation provides a robust, centralized approach to version management that scales well across multiple projects and teams. 