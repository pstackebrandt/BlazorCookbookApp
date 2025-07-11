# Recipe Manifest Implementation Plan

## Progress Overview

### Phase 1: Basic JSON Manifest (Current Focus)
📊 **Progress: 1/22 tasks completed (5%)**

**Planning & Design** ✅ 3/3 completed
- ✅ Strategy selection and documentation  
- ✅ Implementation plan creation
- ✅ Architecture design with clean separation

**Implementation** ⏳ 1/19 completed
- ✅ **T21.1.1**: Tools/RecipeManifestGenerator project structure created
- 🔄 **Current Task**: T21.1.2 Implement ManifestGenerator class reusing existing RecipeScanner logic
- ⏳ Console application development (3 remaining tasks)
- ⏳ RecipeScanner service updates (6 tasks) 
- ⏳ Integration and testing (5 tasks)
- ⏳ Deployment verification (4 tasks)

---

## Table of Contents

1. [Progress Overview](#progress-overview)
2. [Intended Functionality](#intended-functionality)
3. [Implementation Strategy](#implementation-strategy)
4. [Implementation Steps](#implementation-steps)
5. [Architecture & Separation of Concerns](#architecture--separation-of-concerns)
6. [Pros and Cons](#pros-and-cons)
7. [Configuration Options](#configuration-options)
8. [Error Handling](#error-handling)
9. [Testing Strategy](#testing-strategy)
10. [Future Improvements](#future-improvements)

---

## Intended Functionality

**Goal**: Generate recipe metadata during build time and load it at runtime,
eliminating the need to scan `.razor` files in production.

**Current Problem**:
- Development: `RecipeScanner` reads `.razor` files from disk ✅
- Production: `.razor` files don't exist, Browse Recipes page is empty ❌

**Solution**:
- Build-time: Generate `recipes-manifest.json` with all recipe metadata
- Runtime: Load JSON manifest, fallback to file scanning for development
- Configuration: Ability to disable fallback and remove it cleanly

---

## Implementation Strategy

### Phase 1: Basic JSON Manifest (Current Focus)
- Console application that reuses existing `RecipeScanner` logic
- Generate JSON file with recipe metadata
- Update `RecipeScanner` service to load from JSON with file scanning fallback
- Manual execution before publishing

### Phase 2: Integration & Cleanup (Future)
- MSBuild task for automatic generation
- Remove file scanning fallback
- Enhanced error handling and validation

---

## Implementation Steps

### Step 1: Create Recipe Manifest Generator
```
Tools/RecipeManifestGenerator/
├── RecipeManifestGenerator.csproj
├── Program.cs
├── ManifestGenerator.cs
└── Models/
    └── RecipeManifest.cs
```

### Step 2: Update RecipeScanner Service
- Add JSON loading capability
- Implement `IManifestLoader` interface
- Add configuration options for fallback behavior
- Maintain clean separation between JSON and file scanning logic
- Implement recipe visibility filtering (respect `PageVisibleInOverview` property)

### Step 3: Build Process Integration
- Add manifest generation to Production Build Guide
- Include JSON file in publish output
- Update deployment process
- Document PageVisibleInOverview property usage for recipe exclusion
- Add admin view functionality for hidden recipe management

### Step 4: Configuration & Testing
- Add configuration flags in `appsettings.json`
- Create unit tests for manifest functionality
- Validate JSON schema and content
- Implement automatic multi-mode testing (manifest + fallback combinations)
- Test recipe visibility filtering and exclusion logic
- Add PageVisibleInOverview property to sample recipes for testing
- Test admin view functionality (/recipes/admin)

---

## Architecture & Separation of Concerns

### Clean Interface Design
```csharp
public interface IRecipeProvider
{
    Task<List<RecipeInfo>> GetRecipesAsync();
}

public interface IManifestLoader
{
    Task<RecipeManifest?> LoadManifestAsync();
}

public interface IFileScanner
{
    Task<List<RecipeInfo>> ScanFilesAsync();
}
```

### Recipe Metadata Properties
```csharp
@code {
    private static readonly string PageTitle = "Recipe Title";
    private static readonly string PageSummary = "Recipe description";
    private static readonly int PageStars = 5;
    private static readonly bool PageVisibleInOverview = true; // Include in overview (optional, default: true)
}
```

### Enhanced RecipeInfo Model
```csharp
public class RecipeInfo
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int Stars { get; set; }
    public bool VisibleInOverview { get; set; } = true; // New property for recipe exclusion
    public string Chapter { get; set; } = string.Empty;
    public string Recipe { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string? Variant { get; set; }
}
```

### Separation Strategy
- **JSON logic**: Isolated in `ManifestLoader` class
- **File scanning**: Isolated in `FileScanner` class  
- **Configuration**: Controls which provider is used
- **Fallback removal**: Simply remove `FileScanner` registration
- **Recipe visibility**: Controlled per recipe with `PageVisibleInOverview` property

### Admin View for Hidden Recipes
```csharp
@page "/recipes"
@page "/recipes/admin"

private bool ShowHiddenRecipes => 
    NavigationManager.Uri.Contains("/admin") || 
    NavigationManager.Uri.Contains("showHidden=true");

// Filter recipes based on admin view mode
private IEnumerable<RecipeInfo> FilteredRecipes => 
    ShowHiddenRecipes ? allRecipes : allRecipes.Where(r => r.VisibleInOverview);
```

**Admin view characteristics:**
- **Discoverable**: Accessible via direct URL `/recipes/admin`
- **No UI navigation**: No links in NavMenu or page buttons
- **Development friendly**: Easy access for recipe management
- **Visitor awareness**: Visitors can access if they know the URL (for now)

---

## Pros and Cons

### Pros
- **Small package size**: No source files in production
- **Fast startup**: No file system I/O at runtime
- **Secure**: No source code exposure
- **Predictable**: Same data in development and production
- **Maintainable**: Clean separation of concerns

### Cons
- **Build complexity**: Additional build step required
- **Sync risk**: Manifest could become stale if not regenerated
- **Initial effort**: More setup than simple file copying
- **JSON parsing**: Small runtime overhead during startup

---

## Configuration Options

### appsettings.json
```json
{
  "RecipeScanner": {
    "UseManifest": true,
    "ManifestPath": "recipes-manifest.json",
    "EnableFileScanningFallback": true,
    "LogManifestLoading": false
  }
}
```

### Environment-Specific Behavior
- **Development**: File scanning enabled for hot reload
- **Production**: JSON manifest only, no fallback
- **Testing**: Configurable based on test scenario

### Production Environment Simulation (Testing)
```json
{
  "RecipeScanner": {
    "UseManifest": true,
    "EnableFileScanningFallback": false,  // Force manifest-only mode
    "ManifestPath": "test-recipes-manifest.json",
    "LogManifestLoading": true
  }
}
```

---

## Error Handling

### Manifest Loading Failures
- **Missing file**: Fall back to file scanning (if enabled)
- **Invalid JSON**: Log error, fall back to file scanning
- **Empty manifest**: Return empty list, log warning
- **Parsing errors**: Detailed logging for debugging

### File Scanning Fallback
- **Disabled in production**: Fail fast with clear error message
- **Enabled in development**: Seamless fallback with logging
- **Configuration validation**: Ensure settings are consistent

---

## Testing Strategy

### Unit Tests
- **Manifest generation**: Verify JSON structure and content
- **JSON loading**: Test various file conditions and error scenarios
- **Service integration**: Test provider switching and fallback behavior
- **Configuration**: Validate all configuration combinations
- **Recipe visibility**: Test PageVisibleInOverview property handling and exclusion logic

### Integration Tests
- **End-to-end**: Generate manifest → load in RecipeScanner → verify Browse Recipes
- **Production simulation**: Test with published output
- **Error scenarios**: Missing files, corrupt JSON, configuration issues
- **Visibility filtering**: Verify hidden recipes are excluded from Browse Recipes

### Automatic Multi-Mode Testing
```csharp
[Theory]
[InlineData(true, true)]   // Manifest enabled, fallback enabled
[InlineData(true, false)]  // Manifest enabled, fallback disabled (production mode)
[InlineData(false, true)]  // Manifest disabled, fallback enabled (development mode)
public async Task GetRecipesAsync_TestAllConfigurations(bool useManifest, bool enableFallback)
{
    // Arrange: Configure test environment
    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["RecipeScanner:UseManifest"] = useManifest.ToString(),
            ["RecipeScanner:EnableFileScanningFallback"] = enableFallback.ToString()
        })
        .Build();
    
    // Act & Assert: Test all scenarios automatically
}
```

**Benefits:**
- **No manual switching**: All scenarios tested automatically
- **Comprehensive coverage**: Tests all configuration combinations
- **CI/CD friendly**: Runs consistently in build pipeline
- **Regression protection**: Catches issues in any mode

### Production Environment Simulation
- **Manifest-only testing**: Disable file scanning fallback in test configuration
- **Missing razor files**: Test behavior when source files are unavailable
- **Configuration-based**: Use test-specific appsettings to simulate production conditions

```json
{
  "RecipeScanner": {
    "UseManifest": true,
    "EnableFileScanningFallback": false,  // Force manifest-only mode
    "ManifestPath": "test-recipes-manifest.json",
    "LogManifestLoading": true
  }
}
```

---

## Future Improvements

### Phase 2: Build Integration
- **MSBuild task**: Automatic manifest generation during build
- **Incremental builds**: Only regenerate if source files changed
- **CI/CD integration**: Automated manifest validation

### Phase 3: Performance & Features
- **C# class generation**: Compile-time manifest for maximum performance
- **Schema validation**: JSON schema for manifest structure
- **Metadata expansion**: Additional recipe properties (tags, difficulty, etc.)
- **Advanced filtering**: Recipe categories, skill levels, completion status
- **Recipe management**: Bulk visibility controls, recipe status tracking
- **Enhanced admin view**: Recipe visibility toggle UI, batch operations

### Phase 4: Advanced Features
- **Source generators**: Roslyn-based manifest generation
- **Hot reload support**: Live manifest updates in development
- **Caching strategies**: Memory caching for large manifests
- **Admin interface**: Recipe visibility management UI with authentication
- **Search capabilities**: Full-text search within recipe content
- **Recipe lifecycle**: Draft, published, archived states with PageVisibleInOverview integration

---

## File Locations

### Development
- **Console app**: `Tools/RecipeManifestGenerator/`
- **Generated JSON**: `BlazorCookbookApp/wwwroot/recipes-manifest.json`
- **Configuration**: `appsettings.*.json`

### Production
- **Published JSON**: `bin/Publish/wwwroot/recipes-manifest.json`
- **Archive**: Included in deployment ZIP file

---

*Document created 15 Jan 2025, updated 15 Jan 2025 to include PageVisibleInOverview property, admin view functionality, and automatic multi-mode testing.* 