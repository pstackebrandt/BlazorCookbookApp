# Recipe Manifest Implementation Plan

## Progress Overview

### Phase 1: Basic JSON Manifest ✅ COMPLETED
📊 **Progress: 22/22 tasks completed (100%)**

**Planning & Design** ✅ 3/3 completed
- ✅ Strategy selection and documentation  
- ✅ Implementation plan creation
- ✅ Architecture design with clean separation

**Pending Decisions** ✅ Resolved
- ✅ **Debug view security method**: Base64 obfuscation with "show-dark-eyes" key
- ✅ **Reminder visibility**: Warning banner approach confirmed
- ✅ **Naming**: "Debug view" for showing hidden recipes

**Implementation** ✅ 20/20 completed
- ✅ **T21.1.1**: Tools/RecipeManifestGenerator project structure created
- ✅ **T21.1.2**: ManifestGenerator class implemented (reuses existing RecipeScanner logic)
- ✅ **T21.1.3**: RecipeManifest model classes created with JSON serialization
- ✅ **T21.1.4**: JSON serialization and file output functionality implemented
- ✅ **T21.1.5**: PageVisibleInOverview property support and recipe exclusion logic
- ✅ **T21.2.1**: IManifestLoader interface and implementation created
- ✅ **T21.2.2**: Configuration options added in appsettings.json
- ✅ **T21.2.3**: JSON loading with file scanning fallback implemented
- ✅ **T21.2.4**: Logging for manifest loading operations added
- ✅ **T21.2.5**: Recipe visibility filtering in JSON loading implemented
- ✅ **T21.2.6**: Debug view functionality with query parameters (/recipes?admin=true&key=show-dark-eyes)
- ✅ **T21.2.7**: Debug view security implemented (temporarily using plain text key for debugging)
- ✅ **T21.3**: Integration and testing completed successfully
- ✅ **T21.3.2**: Test manifest generation and JSON loading (13 recipes: 10 visible + 3 hidden)
- ✅ **T21.3.5**: Test files created with PageVisibleInOverview=false (2 client + 1 server)
  - **Route fix**: Updated patterns to `/ch01r01test`, `/ch99r02test`, `/ch99r03test` (added test suffix, first file as Chapter 1)
  - **Featured hidden**: First test file has 5 stars and appears in Featured Recipes (hidden in debug mode)
  - **Visibility extraction**: Added PageVisibleInOverview property extraction to RecipeScanner  
  - **UI improvements**: Fixed Visibility column logic, enhanced console logging format
  - **Background colors**: Updated ch01r01test and ch99r03test to use light yellow/beige (alert-warning) instead of red
- ✅ Console application development completed (ManifestGenerator working perfectly)
- ✅ RecipeScanner service updates completed (JSON loading with fallback)
- ✅ Integration and testing completed (build successful, manifest loading working)
- ⏳ **Next**: Deployment verification (4 tasks remaining)
- ⏳ Debug view enhancements (9 low-priority tasks for base64 fix and statistics panel)

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

### Debug View for Hidden Recipes
```csharp
@page "/recipes"

private bool ShowHiddenRecipes => CheckDebugAccess();

private bool CheckDebugAccess()
{
    var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
    var query = QueryHelpers.ParseQuery(uri.Query);
    
    if (query.TryGetValue("admin", out var adminValue) && 
        adminValue == "true" &&
        query.TryGetValue("key", out var keyValue))
    {
        // Base64 obfuscation: "show-dark-eyes" → base64 → reverse
        const string encodedKey = "=c2V5ZS1rcmFkLXdvaHM"; // obfuscated
        var decodedKey = System.Text.Encoding.UTF8.GetString(
            Convert.FromBase64String(new string(encodedKey.Reverse().ToArray())));
        return keyValue == decodedKey;
    }
    
    return false;
}

// Filter recipes based on debug view mode
private IEnumerable<RecipeInfo> FilteredRecipes => 
    ShowHiddenRecipes ? allRecipes : allRecipes.Where(r => r.VisibleInOverview);
```

**Debug view characteristics:**
- **URL format**: `/recipes?admin=true&key=show-dark-eyes`
- **Key obfuscation**: Base64 encoding + simple transformation
- **Easy to remember**: "show-dark-eyes" (general, not app/year specific)
- **Professional**: No plain text key in source code
- **Breakable**: ~2-5 minutes for someone familiar with base64
- **Visible reminder**: Warning banner about future access restrictions

**Security level**: Light obfuscation for professional appearance, not intended as true security

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
- **Enhanced debug view**: Recipe visibility toggle UI, batch operations

### Phase 4: Advanced Features
- **Source generators**: Roslyn-based manifest generation
- **Hot reload support**: Live manifest updates in development
- **Caching strategies**: Memory caching for large manifests
- **Debug interface**: Recipe visibility management UI with authentication
- **Search capabilities**: Full-text search within recipe content
- **Recipe lifecycle**: Draft, published, archived states with PageVisibleInOverview integration

### Debug View Enhancements (Low Priority)
- **Fix base64 obfuscation**: Currently using plain text key for debugging
- **Debug statistics panel**: Structurally separated component showing:
  - Visible/hidden recipe counts (featured vs common)
  - Manifest vs fallback usage status
  - Fallback activation indicator
- **Visual separation**: Clear distinction between debug and regular UI elements
- **Component isolation**: Separate DebugInfoPanel component to prevent cross-contamination

---

## Development Guidelines

### Overengineering Prevention
⚠️ **Rule**: The assistant will warn if solutions become overly complex for the actual problem being solved. Simple solutions are preferred when they adequately address the requirements. Debug enhancements are practical development aids, not overengineering, as they provide valuable system insight.

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

*Document created 15 Jan 2025, updated 15 Jan 2025 to include PageVisibleInOverview property, debug view functionality with base64 obfuscation, and automatic multi-mode testing.*