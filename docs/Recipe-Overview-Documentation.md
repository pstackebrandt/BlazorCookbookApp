# Recipe Overview Feature Documentation

## Purpose

Automatically discovers and displays all Blazor Cookbook recipes in a centralized overview table on the home page. Eliminates manual maintenance by scanning source code for recipe patterns.

## Intent

- **Auto-discovery**: Automatically find recipes by scanning .razor files for route patterns
- **Centralized access**: Single place to see all recipes with quick navigation
- **Metadata extraction**: Show chapter, recipe number, location (Client/Server), and summary
- **Maintenance-free**: No manual list updates needed when adding/removing recipes

## Usage

### For Users

1. Navigate to home page (`/`)
2. View organized table of all recipes
3. Click "Open" button or "Direct link" to navigate to any recipe
4. Recipes are sorted by Chapter → Recipe → Variant

### For Developers

1. Add new recipe with `@page "/ch##r##"` pattern
2. Include H1 or H2 tag for summary
3. Recipe automatically appears in overview on next page load

## Implementation Architecture

### Core Components

**1. RecipeInfo.cs**

- Data model representing recipe metadata
- Properties: Route, Chapter, Recipe, Variant, Location, Summary, FilePath

**2. RecipeScanner.cs**

- Service that scans directories for recipe files
- Uses regex patterns to extract route and summary information
- Scans both Client and Server project directories

**3. Home.razor**

- Updated home page displaying recipe overview table
- Uses InteractiveServer render mode for button functionality
- Includes debug logging to terminal

**4. Program.cs**

- Registers RecipeScanner as scoped service

**5. app.css**

- Custom styling for recipe overview table

### Scanning Logic

**Route Pattern Recognition:**

```regex
@page\s+"(/ch(\d+)r(\d+)(\w*))"
```

- Matches: `@page "/ch01r02"`, `@page "/ch01r03cl"`
- Captures: full route, chapter number, recipe number, variant

**Summary Extraction:**

- First tries H1 tags: `<h1>...</h1>`
- Falls back to H2 tags: `<h2>...</h2>`
- Strips HTML tags for clean text

**Directory Scanning:**

- Server: `BlazorCookbookApp/Components/`
- Client: `BlazorCookbookApp.Client/Pages/`
- Client: `BlazorCookbookApp.Client/Chapters/`

### Data Flow

1. **Page Load** → Home.razor calls `RecipeScanner.GetRecipesAsync()`
2. **Directory Scan** → Scanner recursively finds all .razor files
3. **Pattern Matching** → Extracts recipes matching `/ch##r##` pattern
4. **Metadata Extraction** → Parses chapter, recipe, variant, summary
5. **Sorting** → Orders by Chapter → Recipe → Variant
6. **Display** → Renders table with navigation buttons

## Configuration

### Adding New Search Directories

Modify `RecipeScanner.GetRecipesAsync()` to include additional paths:

```csharp
var newPath = Path.Combine(webRoot, "NewDirectory");
if (Directory.Exists(newPath))
{
    await ScanDirectoryAsync(newPath, "Location", recipes);
}
```

### Customizing Route Patterns

Update regex in `RecipeScanner` constructor:

```csharp
private readonly Regex _routePattern = new(@"your-pattern", RegexOptions.IgnoreCase);
```

## Debugging Features

### Console Logging

- Recipe discovery: `"Found X recipes:"`
- Recipe details: `"- /ch01r02 (Server) - Summary"`
- Navigation attempts: `"Attempting to navigate to: /ch01r02"`
- Error handling: Parse errors and navigation failures

### Direct Links

Each table row includes both:

- Interactive "Open" button (requires render mode)
- Direct HTML anchor link (always works)

## Error Handling

- **File Access Errors**: Logged but don't stop scanning other files.
- **Parse Errors**: Invalid routes/chapters are skipped gracefully.
- **Directory Not Found**: Silently continues with other directories.
- **Navigation Errors**: Logged to console for debugging.

## Performance Considerations

- **Scoped Service**: Recipes loaded once per request
- **Async File Reading**: Non-blocking I/O operations
- **Regex Compilation**: Patterns compiled once on service creation
- **Error Isolation**: Single file failures don't break entire scan

## Future Enhancements

- **Caching**: Store results to avoid rescanning unchanged files
- **File Watching**: Auto-refresh when files change
- **Filtering**: Search/filter recipes by chapter or keyword
- **Categories**: Group recipes by topics beyond chapters
- **Validation**: Verify recipe routes actually work
