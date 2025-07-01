# Recipe Overview Plan

## Purpose

Automatically discovers and displays all Blazor Cookbook recipes in a centralized overview table on a dedicated browse page.
Eliminates manual maintenance by scanning source code for recipe patterns.

## **🔄 ARCHITECTURAL CHANGE: HOME PAGE SEPARATION**

### **New Page Structure (Planned)**
- **Home Page (`/`)**: Project introduction, getting started, featured recipes
- **Browse Recipes Page (`/recipes`)**: Complete recipe overview table with auto-discovery
- **Title**: "Browse Recipes"

### **Rationale**
- **Better UX**: Separate informational content from functional tools
- **Focused Purpose**: Home explains the project, Recipes page provides functionality  
- **Scalability**: Recipes page can grow with search/filter features
- **Web Conventions**: `/recipes` is intuitive and expected by users

### **Implementation Impact**
- **RecipeScanner**: Remains unchanged (core logic preserved)
- **Home.razor**: Will be restructured for project introduction
- **New Recipes.razor**: Will receive current overview table functionality
- **Navigation**: Update NavMenu to include "Browse Recipes" link

### **Testing Strategy for Restructuring**

#### **✅ Phase 1: Pre-Change Testing (COMPLETED)**
**Goal**: Protect business logic during restructuring
**Status**: **COMPLETED - 96 tests passing** ✅
**Scope**: Core components that will remain unchanged

```bash
# Core business logic tests (COMPLETED)
dotnet test --filter "RecipeScanner*"    # 15 tests ✅
dotnet test --filter "*RecipeInfo*"      # 11 tests ✅  
dotnet test --filter "*RecipeUrlService*" # 12 tests ✅
```

**✅ Completed Test Coverage**:
- ✅ **RecipeScannerTests.cs** (15 tests)
  - Route pattern matching (@page "/ch##r##" patterns)
  - Summary extraction from H1/H2 tags
  - Variant handling (e.g., "cl", "wademo")
  - Location setting (Server vs Client)
  - Edge cases and error handling

- ✅ **RecipeInfoTests.cs** (11 tests)
  - Data model properties and validation
  - Default values and null handling
  - Various input scenarios

- ✅ **RecipeUrlServiceTests.cs** (12 tests) 
  - URL parsing and chapter/recipe extraction
  - Title formatting with numbers
  - NavigationManager integration
  - Edge cases and error scenarios

**✅ Technical Issues Resolved**:
- Fixed Moq NavigationManager issues with custom TestNavigationManager
- Resolved line ending differences (\r\n vs \n) on Windows
- Proper reflection-based access to private methods
- All 96 tests passing with 0 failures

**Result**: Core business logic is **fully protected** and ready for restructuring ✅

#### **📋 Phase 2: Post-Change Testing (READY TO BEGIN)**
**Goal**: Validate new structure works correctly
**Scope**: Full test coverage of new architecture

```bash
# Test complete new structure (UPCOMING)
dotnet test  # Full test suite
```

**Upcoming Focus Areas**:
- 🔄 Browse Recipes page functionality at `/recipes`
- 🔄 Home page introduction content at `/`
- 🔄 Navigation flow: Home → Recipes → Individual recipes
- 🔄 RecipeScanner integration with new Browse Recipes page
- 🔄 End-to-end recipe discovery and display
- 🔄 Responsive layout testing
- 🔄 Manual navigation testing

**Current Status**: Ready to proceed with restructuring - core logic is protected ✅

## Intent

- **Auto-discovery**: Automatically find recipes by scanning .razor files for route patterns
- **Centralized access**: Single place to see all recipes with quick navigation
- **Metadata extraction**: Show chapter, recipe number, location (Client/Server), and summary
- **Maintenance-free**: No manual list updates needed when adding/removing recipes

## Current Implementation Status

### ✅ COMPLETED FEATURES

- ✅ Automatic scanning of .razor files for @page patterns
- ✅ Extract summaries from H1 headers in recipe files
- ✅ Direct navigation to recipe pages on click
- ✅ Include non-standard patterns (like /ch01r03cl)
- ✅ Display as structured table format
- ✅ Show Client vs Server project location
- ✅ Simple implementation for quick results

### 🔄 PENDING UPDATES

#### **Phase 2: Home Page Restructuring (IN PROGRESS)**
- [x] Create new Browse Recipes page at `/recipes` route ✅ COMPLETED
- [ ] Move recipe overview table from Home.razor to Recipes.razor  
- [ ] Restructure Home page for project introduction content
- [x] Update navigation menu to include "Browse Recipes" link ✅ COMPLETED
- [ ] Test complete navigation flow and responsive layout

#### **Current Status: Phase 2.1 Complete**
- ✅ **Browse Recipes page created** at `/recipes` with placeholder content
- ✅ **Navigation menu updated** with "Browse Recipes" link
- ✅ **All tests passing** (96/96) - no regressions introduced
- 🔄 **Next step**: Move recipe overview table functionality from Home.razor

#### **Phase 3: Recipe Overview Enhancements (POST-RESTRUCTURING)**  
- [ ] Update Recipe Overview to show clear distinction between render mode pages and demo
- [ ] Verify Recipe Overview integration with all Recipe4 variants
- [ ] Test Recipe Overview responsive layout and navigation on new Browse Recipes page

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
- Current home page displaying recipe overview table
- Uses InteractiveServer render mode for button functionality
- Includes debug logging to terminal
- **Future**: Will be restructured for project introduction content

**4. Recipes.razor** ⭐ **NEW**
- New Browse Recipes page at `/recipes` route
- **Current**: Placeholder content with recipe categories and getting started guide
- **Future**: Will receive recipe overview table functionality from Home.razor
- Uses InteractiveServer render mode for future button functionality

**5. Program.cs**
- Registers RecipeScanner as scoped service

**6. app.css**
- Custom styling for recipe overview table

**7. NavMenu.razor**
- Updated navigation menu with "Browse Recipes" link
- Provides navigation between Home and Browse Recipes pages

### Scanning Logic

**Route Pattern Recognition:**
```regex
@page\s+"(/ch(\d+)r(\d+)(\w*))"
```
- Matches: `@page "/ch01r02"`, `@page "/ch01r03cl"`, `@page "/ch01r04wademo"`
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

## Current Recipe Analysis

Found recipes in these locations:

- **Server**: `BlazorCookbookApp/Components/Recipe2/`, `BlazorCookbookApp/Components/Recipe3/`, `BlazorCookbookApp/Components/Recipe4/`
- **Client**: `BlazorCookbookApp.Client/Pages/Recipe3cl/`, `BlazorCookbookApp.Client/Pages/Recipe4/`,
  `BlazorCookbookApp.Client/Pages/Recipe5/`, `BlazorCookbookApp.Client/Pages/Recipe6/`,
  `BlazorCookbookApp.Client/Chapters/Chapter1/Recipe7/`

### Recipe4 Variants (Render Mode Demonstrations)

- `/ch01r04` - WebAssembly render mode (Client)
- `/ch01r04s` - Server render mode (Server)
- `/ch01r04a` - Auto render mode (Client)
- `/ch01r04wademo` - WebAssembly Features Demo (Client)

## Usage

### For Users

#### **Current Navigation (Phase 2.1)**
1. Navigate to home page (`/`) - shows recipe overview table
2. Navigate to Browse Recipes page (`/recipes`) - shows placeholder content
3. Use navigation menu to switch between pages
4. Click "Open" button or "Direct link" to navigate to any recipe
5. Recipes are sorted by Chapter → Recipe → Variant

#### **Future Navigation (Post-Restructuring)**
1. Navigate to home page (`/`) - shows project introduction and getting started
2. Navigate to Browse Recipes page (`/recipes`) - shows complete recipe overview table
3. Use navigation menu to switch between pages
4. Click "Open" button or "Direct link" to navigate to any recipe
5. Recipes are sorted by Chapter → Recipe → Variant

### For Developers

1. Add new recipe with `@page "/ch##r##"` pattern
2. Include H1 or H2 tag for summary
3. Recipe automatically appears in overview on next page load
4. **Future**: Recipe will appear on Browse Recipes page instead of Home page

## Expected Result

Home page displays a table like:

| Chapter | Recipe | Variant | Location | Summary                                        | Action |
| ------- | ------ | ------- | -------- | ---------------------------------------------- | ------ |
| 1       | 2      |         | Server   | Simple Offer page uses simple Ticket component | [Open] |
| 1       | 3      |         | Server   | Components in Server project                   | [Open] |
| 1       | 3      | cl      | Client   | Components in Client project                   | [Open] |
| 1       | 4      |         | Client   | Render mode InteractiveWebAssembly             | [Open] |
| 1       | 4      | s       | Server   | Render mode InteractiveServer                  | [Open] |
| 1       | 4      | a       | Client   | Render mode InteractiveAuto                    | [Open] |
| 1       | 4      | wademo  | Client   | Interactive WebAssembly Features Demo          | [Open] |
| 1       | 5      |         | Client   | Ensuring component parameter is required       | [Open] |
| 1       | 6      |         | Client   | Selling Tickets with cascading parameters      | [Open] |
| 1       | 7      |         | Client   | Creating components with customizable content  | [Open] |

## Configuration

### Adding New Search Directories

Modify `RecipeScanner.GetRecipesAsync()` to include additional paths:

```