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
- ✅ Extract titles and summaries from PageTitle/PageSummary properties
- ✅ Direct navigation to recipe pages on click
- ✅ Include non-standard patterns (like /ch01r03cl)
- ✅ Display as structured table format
- ✅ Show Client vs Server project location
- ✅ Star rating system with featured recipes section
- ✅ Property-based extraction (no fallback to H1/H2)
- ✅ Comprehensive test coverage (104 tests passing)

### 🔄 NEXT PRIORITIES

#### **Phase 6: Overview Page Enhancements (READY TO BEGIN)**
**Current Status**: Star rating system complete, ready for structural improvements

**Next Steps**:
1. **T12.2.1** Summary truncation for long text
2. **T12.2.2** Responsive table design for mobile
3. **T12.2.3** Basic sorting functionality (Chapter/Recipe/Location)
4. **T12.2.4** Search/filter functionality
5. **T12.2.5.5** Responsive column priority (mobile/tablet/desktop)
6. **T12.3.3** Optimize table spacing and visual hierarchy

#### **✅ COMPLETED PHASES**

**Phase 2: Home Page Restructuring (✅ COMPLETED)**
- Browse Recipes page created at `/recipes` with full recipe overview table
- Home page restructured with project introduction and getting started content
- Navigation menu updated with "Browse Recipes" link
- All tests passing - no regressions introduced

**Phase 3: Recipe Overview Enhancements (✅ COMPLETED)**
- All recipe pages now have PageTitle and PageSummary properties
- RecipeScanner updated to extract from properties only (no H1/H2 fallback)
- Column structure enhanced: Chapter | Recipe | Title | Action | Summary | Location | Stars | Filename
- Property-based extraction shows "unknown" for missing properties

**Phase 4: Property Implementation (✅ COMPLETED)**
- PageTitle and PageSummary properties added to all 10 recipe pages
- Descriptive titles and summaries for better user experience
- Recipe4 variants have clear distinctions between render modes vs demo

**Phase 5: Star Rating System (✅ COMPLETED)**
- Star rating system (1-5 stars) with developer-controlled assignments
- Featured recipes section for 4+ star recipes
- Visual star display (★★★★☆) in main recipe table
- Comprehensive test coverage (104 tests passing)

## **Phase 6: Overview Page Enhancements (NEXT)**

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

## **Phase 5: Star Rating System (✅ COMPLETED)**

### **Purpose**
Help visitors identify more comprehensive and educationally valuable recipes through a developer-controlled star rating system. Featured recipes (4+ stars) get prominent placement to encourage visitors to try bigger, more impactful recipes first.

### **Feature Specification**
- **Rating Scale**: 1-5 stars, developer-controlled
- **Default Value**: 3 stars for most recipes
- **Featured Threshold**: 4+ stars (4 and 5 star recipes)
- **Visual Display**: ★★★★☆ or ★★★★★ (stars without numbers)
- **Missing Property**: Show "unrated" for recipes without PageStars property

### **Initial Star Assignments**
- **5 Stars**: Render mode pages (Recipe4 variants) - high educational value and complexity
- **4 Stars**: WebAssembly demo - good showcase of capabilities
- **3 Stars**: All other recipes (default) - standard educational value

### **Page Layout Changes**
1. **Featured Section** (above main table):
   - Shows recipes with 4+ stars
   - Condensed columns: Title | Action | Summary | Location
   - Prominent placement to encourage trying bigger recipes first

2. **Main Table** (updated):
   - New column order: Chapter | Recipe | Title | Action | Summary | Location | **Stars** | Filename
   - Keep existing Chapter → Recipe sorting (no sorting by stars)
   - Featured recipes appear in both sections

### **Implementation Status: ✅ COMPLETED**
- ✅ **T13.1** Add PageStars property to all recipe pages
- ✅ **T13.2** Update RecipeScanner to extract PageStars property
- ✅ **T13.3** Create featured recipes section component
- ✅ **T13.4** Add Stars column to main recipe table
- ✅ **T13.5** Implement star display formatting (★★★★☆)
- ✅ **T13.6** Update RecipeInfo model to include Stars field
- ✅ **T13.7** Test featured section functionality
- ✅ **T13.8** Update RecipeScannerTests for property-based extraction

### **Final Status: ✅ PHASE 5 COMPLETE**
The star rating system is fully implemented and tested:
- **104 tests passing** (including 33 updated RecipeScannerTests)
- **Featured recipes section** prominently displays 4+ star recipes
- **Star display** working in main recipe table (★★★★☆ format)
- **Property-based extraction** from PageStars properties
- **Application running** successfully at localhost:5000/recipes

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

Browse Recipes page displays a featured section followed by the main table:

### Featured Recipes (4+ Stars)
| Title                               | Action | Summary                                        | Location |
| ----------------------------------- | ------ | ---------------------------------------------- | -------- |
| WebAssembly: Client-side Processing | [Open] | Client-side processing demo                    | Client   |
| Server: Server-side Processing      | [Open] | Server-side processing demo                    | Server   |
| Auto: Adaptive Server-to-Client     | [Open] | Adaptive render mode switching demo            | Client   |
| WebAssembly Demo: Features Showcase | [Open] | Hands-on demonstration of WebAssembly features | Client   |

### All Recipes
| Chapter | Recipe | Title                               | Action | Summary                                           | Location | Stars | Filename        |
| ------- | ------ | ----------------------------------- | ------ | ------------------------------------------------- | -------- | ----- | --------------- |
| 1       | 2      | Simple Offer Page                   | [Open] | Demonstrates basic component usage                | Server   | ★★★☆☆ | Offer           |
| 1       | 3      | Components in Server Project        | [Open] | Shows component usage with WebAssembly mode       | Server   | ★★★☆☆ | Offer           |
| 1       | 3      | Components in Client Project        | [Open] | Demonstrates component parameters and events      | Client   | ★★★☆☆ | Offer           |
| 1       | 4      | WebAssembly: Client-side Processing | [Open] | Client-side processing demo                       | Client   | ★★★★★ | Offer           |
| 1       | 4      | Server: Server-side Processing      | [Open] | Server-side processing demo                       | Server   | ★★★★★ | OfferServer     |
| 1       | 4      | Auto: Adaptive Server-to-Client     | [Open] | Adaptive render mode switching demo               | Client   | ★★★★★ | OfferAuto       |
| 1       | 4      | WebAssembly Demo: Features Showcase | [Open] | Hands-on demonstration of WebAssembly features    | Client   | ★★★★☆ | WebAssemblyDemo |
| 1       | 5      | Required Component Parameters       | [Open] | Ensures parameters are required at compile time   | Client   | ★★★☆☆ | Offer           |
| 1       | 6      | Cascading Parameters                | [Open] | Demonstrates cascading values to child components | Client   | ★★★☆☆ | SellingTickets  |
| 1       | 7      | Customizable Component Content      | [Open] | Shows components with customizable content        | Client   | ★★★☆☆ | Offer           |

## Configuration

### Adding New Search Directories

Modify `RecipeScanner.GetRecipesAsync()` to include additional paths:

```