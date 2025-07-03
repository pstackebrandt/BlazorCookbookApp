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

#### **Phase 2: Home Page Restructuring (COMPLETED)**
- [x] Create new Browse Recipes page at `/recipes` route ✅ COMPLETED
- [x] Move recipe overview table from Home.razor to Recipes.razor ✅ COMPLETED
- [x] Restructure Home page for project introduction content ✅ COMPLETED
- [x] Update navigation menu to include "Browse Recipes" link ✅ COMPLETED
- [x] Test complete navigation flow and responsive layout ✅ COMPLETED

#### **Current Status: Phase 2 Complete**
- ✅ **Browse Recipes page created** at `/recipes` with full recipe overview table
- ✅ **Home page restructured** with project introduction and getting started content
- ✅ **Navigation menu updated** with "Browse Recipes" link
- ✅ **All tests passing** (96/96) - no regressions introduced
- ✅ **RecipeScanner integration verified** - functionality moved successfully
- ✅ **UI improvements completed** - removed Coming Soon alert and direct links
- 🔄 **Next step**: Manual testing and responsive layout verification

#### **Phase 3: Recipe Overview Enhancements (POST-RESTRUCTURING)**
**Goal**: Improve recipe overview clarity and user experience with better titles and distinctions

**Current Recipe4 Variants Analysis**:
- `/ch01r04` - WebAssembly render mode (Client) - "Render mode InteractiveWebAssembly"
- `/ch01r04s` - Server render mode (Server) - "Render mode InteractiveServer"
- `/ch01r04a` - Auto render mode (Client) - "Render mode InteractiveAuto"
- `/ch01r04wademo` - WebAssembly Features Demo (Client) - "Interactive WebAssembly Features Demo"

**Issues to Address**:
1. **Poor Distinction**: All render mode pages have similar titles that don't clearly explain their purpose
2. **Demo Confusion**: The WebAssembly demo page purpose isn't clear from the title
3. **Missing Context**: Users can't easily understand the differences between render modes

**Step-by-Step Enhancement Plan**:

**Step 1: Improve Recipe4 Titles and Summaries**
this is done!
- Update PageTitle and PageSummary properties in all Recipe4 variants
- Make titles more descriptive and user-friendly (shorter versions recommended)
- Add clear distinctions between render modes vs demo
- **Recommended Titles**:
  - WebAssembly: "WebAssembly: Client-side Processing"
  - Server: "Server: Server-side Processing"
  - Auto: "Auto: Adaptive Server-to-Client"
  - Demo: "WebAssembly Demo: Features Showcase"
- **Expected Result**: Clear, descriptive titles that explain each page's purpose

**Step 2: Test Recipe Overview Integration**
- Run tests to ensure RecipeScanner still works with updated titles
- Verify the overview table displays improved information
- Check that all Recipe4 variants appear correctly
- **Expected Result**: All tests pass, overview table shows improved information

**Step 3: Manual Testing and Responsive Layout**
- Test navigation flow between Home → Browse Recipes → Individual recipes
- Verify responsive layout on different screen sizes
- Test all Recipe4 variants work correctly
- **Expected Result**: Smooth navigation and responsive design

**Step 4: Update Documentation**
- Update planning docs to reflect Phase 3 completion
- Document the improved recipe overview experience
- **Expected Result**: Complete documentation of enhancements

**Current Status**: ✅ **Phase 3 COMPLETED** - Recipe4 titles improved and tested

**New Column Structure (Planned)**:
| Chapter | Recipe | Title             | Action | Summary                      | Location | Filename    |
| ------- | ------ | ----------------- | ------ | ---------------------------- | -------- | ----------- |
| 1       | 2      | Simple Offer Page | [Open] | Uses simple Ticket component | Server   | Offer       |
| 1       | 4      | WebAssembly Mode  | [Open] | Client-side processing demo  | Client   | Offer       |
| 1       | 4      | unknown           | [Open] | unknown                      | Server   | OfferServer |

**Scanner Behavior**:
- Extract **only** `PageTitle` and `PageSummary` properties from recipe pages
- Show "unknown" if properties are missing (indicates need to add them)
- No fallback to H1/H2 extraction
- Filename shows name without extension or path (e.g., "Offer", "OfferServer")

## **Phase 4: Overview Page Enhancements (NEXT)**

### **Priority 1: Structural Improvements**
- **T12.2.1** Summary truncation for long text
- **T12.2.2** Responsive table design for mobile
- **T12.2.3** Basic sorting functionality (Chapter/Recipe/Location)
- **T12.2.4** Search/filter functionality
- **T12.2.5** Column structure enhancements
  - Add PageTitle property to all recipe pages (no fallback to H1/H2)
  - Add PageSummary property to all recipe pages (dedicated summaries)
  - Update RecipeScanner to extract only PageTitle and PageSummary properties (show "unknown" if missing)
  - Reorder columns: Chapter | Recipe | Title | Action | Summary | Location | Filename
  - Implement responsive column priority (mobile/tablet/desktop)

### **Priority 2: Optical Improvements**
- **T12.3.3** Optimized table spacing and visual hierarchy

### **Priority 3: Code Quality (Later)**
- **T12.1.1** Console logging cleanup (remove debug statements that are not needed)
- **T12.1.2** Error handling enhancement (show user-visible error messages, if this is useful)

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
- **Current**: Project introduction page with getting started content
- **Previous**: Displayed recipe overview table (moved to Recipes.razor)
- Uses InteractiveServer render mode for future functionality
- **Content**: About project, getting started guide, recipe categories, link to Browse Recipes

**4. Recipes.razor** ⭐ **COMPLETED**
- Browse Recipes page at `/recipes` route
- **Current**: Complete recipe overview table with auto-discovery functionality
- **Previous**: Placeholder content (replaced with full functionality from Home.razor)
- Uses InteractiveServer render mode for button functionality
- **Content**: Clean recipe table with Open buttons only (no direct links)
- **UI**: Streamlined design without Coming Soon alerts or placeholder content

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

#### **Current Navigation (Phase 2 Complete)**
1. Navigate to home page (`/`) - shows project introduction and getting started
2. Navigate to Browse Recipes page (`/recipes`) - shows complete recipe overview table
3. Use navigation menu to switch between pages
4. Click "Open" button or "Direct link" to navigate to any recipe
5. Recipes are sorted by Chapter → Recipe → Variant

#### **Previous Navigation (Before Restructuring)**
1. Navigate to home page (`/`) - showed recipe overview table
2. No dedicated Browse Recipes page
3. All functionality was on the home page

### For Developers

1. Add new recipe with `@page "/ch##r##"` pattern
2. Include H1 or H2 tag for summary
3. Recipe automatically appears in overview on Browse Recipes page
4. **Current**: Recipe appears on `/recipes` page (Browse Recipes)
5. **Previous**: Recipe appeared on `/` page (Home)

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