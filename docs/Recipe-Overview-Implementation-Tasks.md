# Recipe Overview Implementation Tasks

## Goal

Create an overview on the Home page showing all implemented recipes, organized by chapter and recipe number, with direct navigation links.

## Requirements Summary

- ✅ Automatic scanning of .razor files for @page patterns
- ✅ Extract summaries from H1 headers in recipe files  
- ✅ Direct navigation to recipe pages on click
- ✅ Include non-standard patterns (like /ch01r03cl)
- ✅ Display as structured table format
- ✅ Show Client vs Server project location
- ✅ Keep implementation simple for quick results

## Current Recipe Analysis

Found recipes in these locations:

- **Server**: `BlazorCookbookApp/Components/Recipe2/`, `BlazorCookbookApp/Components/Recipe3/`
- **Client**: `BlazorCookbookApp.Client/Pages/Recipe3cl/`, `BlazorCookbookApp.Client/Pages/Recipe4/`, `BlazorCookbookApp.Client/Pages/Recipe5/`, `BlazorCookbookApp.Client/Pages/Recipe6/`, `BlazorCookbookApp.Client/Chapters/Chapter1/Recipe7/`

## Implementation Steps

### Step 1: Create Recipe Model

Create a simple model class to represent recipe information:

```csharp
public class RecipeInfo
{
    public string Route { get; set; } // e.g., "/ch01r02"
    public int Chapter { get; set; }
    public int Recipe { get; set; }
    public string? Variant { get; set; } // e.g., "cl" for client
    public string Location { get; set; } // "Client" or "Server"
    public string Summary { get; set; }
    public string FilePath { get; set; }
}
```

### Step 2: Create Recipe Scanner Service

Create a service that scans for recipes:

- Scan directories: `Components/`, `BlazorCookbookApp.Client/Pages/`, `BlazorCookbookApp.Client/Chapters/`
- Find .razor files with @page directives matching pattern `/ch\d+r\d+.*`
- Extract chapter/recipe numbers using regex
- Determine Client/Server based on file path
- Extract summary from first H1 tag content

### Step 3: Update Home Page

Modify `BlazorCookbookApp/Components/Pages/Home.razor`:

- Inject the recipe scanner service
- Display recipes in a table format with columns:
  - Chapter
  - Recipe  
  - Variant (if any)
  - Location (Client/Server)
  - Summary
  - Action (Navigate button/link)

### Step 4: Styling

Add basic CSS for the recipe table to make it look clean and organized.

## Technical Approach (Simple & Quick)

### File Scanning Strategy

- Use `Directory.GetFiles()` with recursive search
- Read file content line by line until finding @page directive
- Use simple string parsing/regex for route extraction
- Use basic HTML parsing for H1 content (or simple regex)

### Service Registration

- Register as Scoped service in Program.cs
- Load recipes once per request (simple approach)

### Route Pattern Parsing

Use regex pattern: `@page\s+"(/ch(\d+)r(\d+)(\w*))"`

- Group 1: Full route (e.g., "/ch01r02")
- Group 2: Chapter number (e.g., "01")  
- Group 3: Recipe number (e.g., "02")
- Group 4: Variant (e.g., "cl" or empty)

### Summary Extraction

Look for first `<h1>` tag after the @page directive and extract inner text.

## Files to Create/Modify

1. **New**: `Services/RecipeInfo.cs` - Model class
2. **New**: `Services/RecipeScanner.cs` - Scanner service  
3. **Modify**: `Program.cs` - Register service
4. **Modify**: `Components/Pages/Home.razor` - Add recipe overview table
5. **Modify**: `wwwroot/app.css` - Add table styling

## Expected Result

Home page will display a table like:

| Chapter | Recipe | Variant | Location | Summary                                        | Action |
| ------- | ------ | ------- | -------- | ---------------------------------------------- | ------ |
| 1       | 2      |         | Server   | Simple Offer page uses simple Ticket component | [Open] |
| 1       | 3      |         | Server   | Components in Server project                   | [Open] |
| 1       | 3      | cl      | Client   | Components in Client project                   | [Open] |
| 1       | 4      |         | Client   | Detecting rendermode at runtime                | [Open] |
| 1       | 5      |         | Client   | Ensuring component parameter is required       | [Open] |
| 1       | 6      |         | Client   | Selling Tickets with cascading parameters      | [Open] |
| 1       | 7      |         | Client   | Creating components with customizable content  | [Open] |

## Notes

- Keep error handling minimal for quick implementation
- If file parsing fails for any recipe, log and continue with others
- Focus on getting basic functionality working first
- Can enhance later with caching, better parsing, etc.
