# Recipe Manifest Implementation Plan

## Table of Contents

1. [Intended Functionality](#intended-functionality)
2. [Implementation Strategy](#implementation-strategy)
3. [Implementation Steps](#implementation-steps)
4. [Architecture & Separation of Concerns](#architecture--separation-of-concerns)
5. [Pros and Cons](#pros-and-cons)
6. [Configuration Options](#configuration-options)
7. [Error Handling](#error-handling)
8. [Testing Strategy](#testing-strategy)
9. [Future Improvements](#future-improvements)

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

### Step 3: Build Process Integration
- Add manifest generation to Production Build Guide
- Include JSON file in publish output
- Update deployment process

### Step 4: Configuration & Testing
- Add configuration flags in `appsettings.json`
- Create unit tests for manifest generation
- Validate JSON schema and content

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

### Separation Strategy
- **JSON logic**: Isolated in `ManifestLoader` class
- **File scanning**: Isolated in `FileScanner` class  
- **Configuration**: Controls which provider is used
- **Fallback removal**: Simply remove `FileScanner` registration

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

### Integration Tests
- **End-to-end**: Generate manifest → load in RecipeScanner → verify Browse Recipes
- **Production simulation**: Test with published output
- **Error scenarios**: Missing files, corrupt JSON, configuration issues

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

### Phase 4: Advanced Features
- **Source generators**: Roslyn-based manifest generation
- **Hot reload support**: Live manifest updates in development
- **Caching strategies**: Memory caching for large manifests

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

*Document created 15 Jan 2025 to guide Recipe Manifest implementation.* 