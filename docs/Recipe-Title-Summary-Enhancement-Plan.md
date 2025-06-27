# Recipe Title and Summary Enhancement Plan

## Problem Statement

Currently, the Recipe Overview system shows identical summaries for recipes that should be distinct (e.g., both `/ch01r04` and `/ch01r04s` show "@RecipeUrlService.GetTitleWithNumbers(_title)"). The RecipeScanner extracts summaries from H1 tags, but many recipes use dynamic titles that don't provide meaningful descriptions.

## Proposed Solution

Implement a dual-content system with:
1. **Hardcoded Title**: Descriptive title without chapter/recipe numbers
2. **Hardcoded Summary**: Concise description of what the recipe demonstrates

## Content Structure Design

### Current State
```razor
<PageTitle>@RecipeUrlService.GetTitleWithNumbers(_title)</PageTitle>
<h1>@RecipeUrlService.GetTitleWithNumbers(_title)</h1>
```
Where `_title = "Render modes"` results in generic summaries.

### Proposed Structure
```razor
<PageTitle>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</PageTitle>
<h1>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</h1>
<p class="recipe-summary lead">@RECIPE_SUMMARY</p>

@code {
    // Recipe metadata constants (used by RecipeScanner)
    private const string RECIPE_TITLE = "Render modes";
    private const string RECIPE_SUMMARY = "Client-side rendering with WebAssembly download and browser-based interactivity";
}
```

### Content Guidelines

#### Title Guidelines
- **Length**: 3-8 words maximum
- **Format**: Descriptive noun phrase
- **Purpose**: Clear identification of recipe focus
- **Display**: Used in both page H1 (with chapter/recipe numbers) and Recipe Overview table
- **Examples**:
  - "Render modes" (will display as "Render modes (Chapter 1, Recipe 4)")
  - "Component parameters" 
  - "Server components"

#### Summary Guidelines
- **Length**: 10-25 words (1-2 sentences)
- **Format**: Action-oriented description
- **Purpose**: Explain what the recipe teaches/demonstrates
- **Examples**:
  - "Demonstrates client-side rendering with WebAssembly download and browser-based interactivity"
  - "Shows server-side rendering with SignalR communication for real-time updates"
  - "Validates required component parameters with custom error handling"

## Recipe Overview Display Strategy

### Full Display (Desktop)
```
| Chapter | Recipe | Name         | Action | Location | Summary                                  | Variant |
| ------- | ------ | ------------ | ------ | -------- | ---------------------------------------- | ------- |
| 1       | 4      | Render modes | [Open] | Client   | Client-side rendering with WASM download |         |
| 1       | 4      | Render modes | [Open] | Server   | Server-side rendering with SignalR       | s       |
```

### Responsive Display (Mobile/Tablet)
- **Name**: Always show full recipe name
- **Summary**: Truncate with ellipsis using Bootstrap text-truncate
- **Mobile**: Approximately 50 characters max for summary
- **Tablet**: Approximately 80 characters max for summary
- **Expandable**: Click/tap to show full summary (future enhancement)

## Implementation Architecture

### Phase 1: RecipeScanner Enhancement

#### Current Extraction Logic
```csharp
// Extracts from H1/H2 tags
private string ExtractSummary(string content)
{
    var h1Match = _h1Pattern.Match(content);
    // Returns H1 content as summary
}
```

#### Enhanced Extraction Logic
```csharp
// Extract both title and summary from Razor constants
private (string title, string summary) ExtractTitleAndSummary(string content)
{
    // Look for: private const string RECIPE_TITLE = "...";
    // Look for: private const string RECIPE_SUMMARY = "...";
    // Parse string literal values from constant declarations
}
```

### Phase 2: Recipe File Structure

#### Option B: Razor Constants (SELECTED)
```razor
@page "/ch01r04"
@rendermode InteractiveWebAssembly

<PageTitle>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</PageTitle>
<h1>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</h1>
<p class="recipe-summary lead">@RECIPE_SUMMARY</p>

@code {
    // Recipe metadata constants (used by RecipeScanner)
    private const string RECIPE_TITLE = "Render modes";
    private const string RECIPE_SUMMARY = "Demonstrates client-side rendering with WebAssembly download and browser-based interactivity";
    
    // Other existing component code...
}
```

#### Option A: HTML Comments (Alternative)
```razor
@page "/ch01r04"
@rendermode InteractiveWebAssembly
<!-- RECIPE_TITLE: Render modes -->
<!-- RECIPE_SUMMARY: Demonstrates client-side rendering with WebAssembly download and browser-based interactivity -->

<PageTitle>@RecipeUrlService.GetTitleWithNumbers(_title)</PageTitle>
<h1>@RecipeUrlService.GetTitleWithNumbers(_title)</h1>
<p class="recipe-summary text-muted">@_summary</p>
```

#### Option C: JSON Metadata Block (Future-Proof)
```razor
@*
RECIPE_METADATA: {
  "title": "InteractiveWebAssembly Render Mode",
  "summary": "Demonstrates client-side rendering with WebAssembly download and browser-based interactivity",
  "tags": ["render-mode", "webassembly", "client-side"],
  "difficulty": "intermediate"
}
*@
```

### Phase 3: RecipeInfo Model Enhancement

#### Current Model
```csharp
public class RecipeInfo
{
    public string Summary { get; set; } = string.Empty; // Currently used for both
}
```

#### Enhanced Model
```csharp
public class RecipeInfo
{
    public string Name { get; set; } = string.Empty;         // NEW: Recipe name (e.g., "Render modes")
    public string Summary { get; set; } = string.Empty;      // Enhanced: Hardcoded summary
    // Note: DisplayTitle with chapter/recipe numbers will be generated in the view
}
```

## Content Strategy for Existing Recipes

### Recipe 4 Variants
| Route     | Name         | Summary                                                                |
| --------- | ------------ | ---------------------------------------------------------------------- |
| /ch01r04  | Render modes | Client-side rendering with WebAssembly download and local processing   |
| /ch01r04s | Render modes | Server-side rendering with SignalR communication for real-time updates |
| /ch01r04a | Render modes | Adaptive rendering - server-first, then client-side after WASM loads   |

### Other Existing Recipes (Examples)
| Route      | Current H1          | Proposed Name     | Proposed Summary                              |
| ---------- | ------------------- | ----------------- | --------------------------------------------- |
| /ch01r02   | "Simple components" | Simple components | Basic component parameters and event handling |
| /ch01r03   | "Components"        | Components        | Component architecture in server project      |
| /ch01r03cl | "Components"        | Components        | Component architecture in client project      |

## Implementation Tasks

### T1: Planning and Design ✅ COMPLETED
- [x] Create comprehensive planning document
- [x] Define content guidelines and structure
- [x] Choose implementation approach (Razor variables selected)
- [x] Plan RecipeScanner enhancements
- [x] Define table column order: Chapter | Recipe | Name | Action | Location | Summary | Variant

### T2: RecipeScanner Enhancement
- [ ] **T2.1**: Update RecipeInfo model with separate Name and Summary properties
- [ ] **T2.2**: Enhance extraction logic to parse both name and summary from Razor variables
- [ ] **T2.3**: Add regex patterns to extract string values from constant declarations
- [ ] **T2.4**: Update RecipeScanner regex patterns for new extraction
- [ ] **T2.5**: Add fallback logic for recipes without new metadata format

### T3: Recipe Overview Display Updates
- [ ] **T3.1**: Update Home.razor table with new column order: Chapter | Recipe | Name | Action | Location | Summary | Variant
- [ ] **T3.2**: Implement responsive summary truncation using Bootstrap (50 chars mobile, 80 chars tablet)
- [ ] **T3.3**: Add CSS classes for recipe-summary styling
- [ ] **T3.4**: Test table layout with longer names and summaries

### T4: Recipe File Updates - Recipe 4 Series (PROOF OF CONCEPT)
- [ ] **T4.1**: Update `/ch01r04` (Client) - add RECIPE_TITLE and RECIPE_SUMMARY constants, use directly in markup
- [ ] **T4.2**: Update `/ch01r04s` (Server) - add RECIPE_TITLE and RECIPE_SUMMARY constants, use directly in markup
- [ ] **T4.3**: Replace existing _title variable usage with RECIPE_TITLE constant
- [ ] **T4.4**: Add recipe summary display as subtitle using `<p class="recipe-summary lead">@RECIPE_SUMMARY</p>`
- [ ] **T4.5**: Test RecipeScanner extraction for all Recipe 4 variants
- [ ] **T4.6**: Verify Recipe Overview shows distinct summaries for each variant

### T5: Content Creation for All Existing Recipes
- [ ] **T5.1**: Audit all existing recipes discovered by RecipeScanner
- [ ] **T5.2**: Create comprehensive content document with proposed names/summaries for all recipes
- [ ] **T5.3**: Include both technical aspects and user benefits in summaries
- [ ] **T5.4**: Update Recipe 2 series (/ch01r02) 
- [ ] **T5.5**: Update Recipe 3 series (/ch01r03, /ch01r03cl)
- [ ] **T5.6**: Update Recipe 5 series (/ch01r05)
- [ ] **T5.7**: Update Recipe 6 series (/ch01r06)
- [ ] **T5.8**: Update Recipe 7 series (/ch01r07)

### T6: Testing and Validation
- [ ] **T6.1**: Test RecipeScanner with updated extraction logic
- [ ] **T6.2**: Verify Recipe Overview displays distinct titles and summaries
- [ ] **T6.3**: Test responsive behavior on different screen sizes
- [ ] **T6.4**: Validate PageTitle generation still works correctly
- [ ] **T6.5**: Test backward compatibility with recipes not yet updated

### T7: Documentation and Polish
- [ ] **T7.1**: Update Recipe Overview documentation with new structure
- [ ] **T7.2**: Create content guidelines for future recipe authors
- [ ] **T7.3**: Add CSS styling for recipe summaries
- [ ] **T7.4**: Update README with new recipe metadata format

## Questions for Clarification

### 1. Recipe Summary Display Style ✅ DECIDED
**Decision**: Use Bootstrap `lead` class for prominent summary display:
- `<p class="recipe-summary lead">@_summary</p>` (Bootstrap lead text - larger, prominent)

### 2. RecipeScanner Variable Extraction ✅ DECIDED
**Decision**: Use Option A - Exact constant names only:
- Look for exactly: `private const string RECIPE_TITLE = "...";`
- Look for exactly: `private const string RECIPE_SUMMARY = "...";`
- Use constants directly in markup (no intermediate private variables)
- Clear, predictable naming convention that avoids conflicts

### 3. Table Responsive Behavior ✅ DECIDED
**Decision**: Keep it simple with Bootstrap responsive table:
- Use `table-responsive` class for horizontal scroll on small screens
- Truncate summary text based on screen size (50 chars mobile, 80 chars tablet)
- Maintain table structure across all devices for consistency

### 5. Content Audit Scope ✅ DECIDED
**Decision**: Create comprehensive content audit for all existing recipes:
- Propose names/summaries for all discovered recipes
- Content can be adjusted later based on feedback
- Proceed with full content creation after Recipe 4 proof-of-concept

### 4. Summary Length Guidelines ✅ DECIDED
**Decision**: Include both technical aspects and user benefits:
- Technical details: "WebAssembly download", "SignalR communication"
- User benefits: "faster interactions", "real-time updates"
- Format: "Technical approach - user benefit" or combine naturally

## Success Criteria

- [ ] Recipe Overview shows distinct, meaningful titles for each recipe
- [ ] Summaries clearly explain what each recipe demonstrates
- [ ] Responsive design handles varying content lengths gracefully
- [ ] PageTitle generation continues to work with chapter/recipe numbers
- [ ] System remains maintainable for future recipe additions
- [ ] Backward compatibility maintained during transition period

## Final Implementation Approach ✅ DECIDED

### Constants-Based Architecture
- **Use constants directly**: `RECIPE_TITLE` and `RECIPE_SUMMARY` used directly in markup
- **No intermediate variables**: Eliminates `_title` and `_summary` private variables
- **Clear naming**: `RECIPE_TITLE` and `RECIPE_SUMMARY` are descriptive and avoid conflicts
- **RecipeScanner extraction**: Parse string literals from constant declarations using regex

### Markup Pattern
```razor
<PageTitle>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</PageTitle>
<h1>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</h1>
<p class="recipe-summary lead">@RECIPE_SUMMARY</p>

@code {
    private const string RECIPE_TITLE = "Render modes";
    private const string RECIPE_SUMMARY = "Client-side rendering with WebAssembly download and browser-based interactivity";
}
```

### RecipeScanner Patterns
```csharp
// Extract title: private const string RECIPE_TITLE = "value";
private readonly Regex _titlePattern = new(@"private\s+const\s+string\s+RECIPE_TITLE\s*=\s*""([^""]+)""", RegexOptions.IgnoreCase);

// Extract summary: private const string RECIPE_SUMMARY = "value";
private readonly Regex _summaryPattern = new(@"private\s+const\s+string\s+RECIPE_SUMMARY\s*=\s*""([^""]+)""", RegexOptions.IgnoreCase);
```

## Risk Mitigation

### Risk 1: Content Consistency
**Mitigation**: Create clear content guidelines and review process

### Risk 2: Breaking Changes
**Mitigation**: Implement with backward compatibility and gradual migration

### Risk 3: Screen Size Variations
**Mitigation**: Test thoroughly on multiple device sizes and implement responsive truncation

### Risk 4: Maintenance Overhead
**Mitigation**: Choose simple, sustainable metadata format and provide clear documentation 