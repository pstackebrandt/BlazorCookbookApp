# Tasks

## Todo

- [x] T20 Fix Recipe Overview Build Error (NETSDK1022) ✅ COMPLETED
- [x] T20.1 Resolve duplicate Content items error caused by manual .razor file includes conflicting with SDK defaults ✅ COMPLETED
- [x] T20.2 Revert T18.1 changes: Remove manual Content includes from BlazorCookbookApp.csproj ✅ COMPLETED
- [x] T20.3 Test build after removing manual includes (should resolve NETSDK1022 error) ✅ COMPLETED
- [x] T20.4 Update Recipe-Overview-Fix-Strategies.md: mark strategy 1 as non-viable due to SDK conflicts ✅ COMPLETED
- [x] T20.5 Test production build locally: Confirm Browse Recipes page is empty (expected behavior) ✅ COMPLETED
- [x] T20.6 Choose next strategy approach for Recipe Overview page functionality ✅ COMPLETED
- [x] T20.7 Create Recipe Manifest Implementation Plan document ✅ COMPLETED

- [ ] T21 Implement Build-time Recipe Manifest (Phase 1)
- [ ] T21.1 Create Recipe Manifest Generator console application
- [ ] T21.1.3 Create RecipeManifest model classes
- [ ] T21.1.4 Add JSON serialization and file output
- [ ] T21.1.5 Add PageVisibleInOverview property support and recipe exclusion logic
- [ ] T21.2 Update RecipeScanner service for JSON loading
- [ ] T21.2.1 Create IManifestLoader interface and implementation
- [ ] T21.2.2 Add configuration options in appsettings.json
- [ ] T21.2.3 Implement JSON loading with file scanning fallback
- [ ] T21.2.4 Add logging for manifest loading operations
- [ ] T21.2.5 Implement recipe visibility filtering in JSON loading (PageVisibleInOverview)
- [ ] T21.2.6 Add admin view functionality for hidden recipe management (/recipes/admin)
- [ ] T21.3 Integration and testing
- [ ] T21.3.1 Add manifest generation to Production Build Guide
- [ ] T21.3.2 Test manifest generation and JSON loading
- [ ] T21.3.3 Create unit tests for manifest functionality
- [ ] T21.3.4 Test production build with JSON manifest
- [ ] T21.3.5 Add PageVisibleInOverview property to recipe pages (test with hidden recipes)
- [ ] T21.3.6 Implement automatic multi-mode testing (manifest + fallback combinations)
- [ ] T21.3.7 Test recipe visibility filtering and exclusion logic
- [ ] T21.3.8 Test admin view functionality and hidden recipe access
- [ ] T21.4 Deployment verification
- [ ] T21.4.1 Deploy with manifest and verify Browse Recipes page works
- [ ] T21.4.2 Test error scenarios and fallback behavior
- [ ] T21.4.3 Verify hidden recipes are excluded from Browse Recipes in production
- [ ] T21.4.4 Verify admin view works for hidden recipe management in production

- [x] T15 Documentation Reorganization Series ✅ COMPLETED
- [x] T15.1 Create new documentation structure with topic-based folders
  (deployment, development, features, project-management) ✅ COMPLETED
- [x] T15.2 Create archive structure with 2025-q1 time-based subdivision
  (completed-features, deferred-projects, documentation-history) ✅ COMPLETED
- [x] T15.3 Move completed feature documentation to archive/2025-q1/completed-features/
  (Recipe4-Optimization-Plan.md, Recipe-Overview-Plan.md) ✅ COMPLETED
- [x] T15.4 Move Documentation-Reorganization-Summary.md to archive/2025-q1/documentation-history/ ✅ COMPLETED
- [x] T15.5 Organize remaining documentation files into topic-based folders
  (deployment, development, features, project-management) ✅ COMPLETED
- [x] T15.6 Create Documentation-Reorganization-2025.md with new structure
  explanation and migration record ✅ COMPLETED
- [x] T15.7 Create comprehensive README.md in docs/ explaining structure,
  conventions, and usage guidelines ✅ COMPLETED
- [x] T15.8 Remove empty deployment-debugging/ folder ✅ COMPLETED
- [x] T15.9 Move and rename remaining documentation files:
  VERSION-IMPLEMENTATION-GUIDE.md → development/Version-Implementation-Guide.md,
  DEVELOPMENT_TIPS.md → development/Development-Tips.md,
  VERSIONING.md → project-management/Versioning-Strategy.md ✅ COMPLETED
- [x] T15.10 Fix archive structure consistency: move old completed-projects
  and exported-chats to 2025-q1 time-based structure,
  rename HEALTH-CHECK-IMPLEMENTATION.md → Health-Check-Implementation.md ✅ COMPLETED

- [ ] T14 Azure Deployment Preparation
- [x] T14.0 Git Branch Management (merge current work, create deployment branch) ✅ COMPLETED
- [x] T14.1.6 Learning Journey consistency fixes (perspective, book titles, remove bUnit) ✅ COMPLETED
- [x] T14.1.7 Update Resources page with bUnit under development tools ✅ COMPLETED
- [x] T14.1.8 Prepare Home page content with project purpose text ✅ COMPLETED
- [x] T14.1.9 Remove About link from MainLayout.razor ✅ COMPLETED
- [x] T14.2 Update README.md (deployment info, attribution, educational purpose) ✅ COMPLETED
- [x] T14.3 Production Configuration Review (appsettings, security, performance) ✅ COMPLETED
- [x] T14.3.10 Review Program.cs for Azure App Service compatibility
  (HTTPS redirection, static files) ✅ COMPLETED
- [ ] T14.4 Application Performance Optimization (bundle size, static assets, caching)
- [x] T14.7 Home Page Consistency and Content Optimization ✅ COMPLETED
- [x] T14.7.1 Remove "From Cookbook to Code" duplication from Resources page ✅ COMPLETED
- [x] T14.7.2 Replace generic "About This Project" with personal project purpose ✅ COMPLETED
- [x] T14.7.3 Add prominent navigation links to Learning Journey and Resources ✅ COMPLETED
- [x] T14.7.4 Remove "Coming Soon" and replace with continuous expansion messaging ✅ COMPLETED
- [x] T14.7.5 Add Featured Highlights section with 2 top recipes as teaser ✅ COMPLETED
- [x] T14.7.6 Home page content refinements (remove comprehensive, fix links, add book link, remove low-value sections) ✅ COMPLETED
- [x] T14.7.7 Fix book links (updated Packt URLs) and Auto page route (/ch01r04a) ✅ COMPLETED
- [x] T14.8 Add repository link to Home page Quick Start section ✅ COMPLETED
- [x] T14.5 Add Impress (Legal Notice) [PRE-DEPLOYMENT - navigation only] ✅ COMPLETED
- [x] T14.5.1 Create bilingual Impress pages (English /impress, German /impressum with English filename) ✅ COMPLETED
- [x] T14.5.2 Add Legal Notice link to NavMenu.razor with separator and fixed icons ✅ COMPLETED
- [x] T14.5.3 Test navigation and responsive design ✅ COMPLETED
- [x] T14.5.4 Fix missing icons for Browse Recipes and Legal Notice ✅ COMPLETED

- [ ] T18 Recipe Overview Page – Publish Strategies
- [x] T18.1 Revert quick-fix strategy: Remove .razor source copy due to SDK conflicts ✅ COMPLETED
- [x] T18.2 Choose build-time manifest strategy for implementation ✅ COMPLETED
- [x] T18.3 Create detailed implementation plan with clean architecture ✅ COMPLETED
- [ ] T18.4 Implement build-time manifest (see T21 tasks for details)

- [x] T19 Consolidate production build docs ✅ COMPLETED
- [x] T19.1 Add Production-Build-Guide.md (single source) ✅ COMPLETED
- [x] T19.2 Refactor Azure-Deployment-Checklist to reference guide ✅ COMPLETED
- [x] T19.3 Refactor Azure-Deployment-Preparation-Plan to reference guide ✅ COMPLETED
- [x] T19.4 Add guide references to remaining deployment docs ✅ COMPLETED

- [ ] T15 Mobile Responsiveness and Testing
- [x] T15.1 Fix main recipe table mobile responsiveness by adding table-responsive wrapper ✅ COMPLETED
- [x] T15.3 Implement mobile-first column priority system for recipe tables
  (Essential: Title/Action/Stars, Secondary: Summary/Location, Tertiary: Chapter/Recipe/Filename) ✅ COMPLETED
- [ ] T15.6 Manual mobile testing on actual phone devices (post-deployment verification)
- [x] T15.7 Desktop browser mobile emulation testing (Chrome DevTools, Firefox Responsive Design Mode) ✅ COMPLETED
- [x] T15.4 FUTURE: Review and optimize small font sizes for mobile readability
  (0.75rem badges → 0.85rem, 0.9rem table text → 1rem)
- [x] T15.5 FUTURE: Add touch-friendly mobile enhancements
  (44px tap targets, improved spacing, swipe gestures)

- [ ] T12 Overview Page Structural and Optical Improvements
- [ ] T12.2 Implement structural improvements for better data display
- [ ] T12.2.1 Add summary truncation for long text
- [x] T12.2.2 Implement responsive table design for mobile ✅ COMPLETED
- [ ] T12.2.3 Add basic sorting functionality (Chapter/Recipe/Location)
- [ ] T12.2.4 Add search/filter functionality
- [ ] T12.2.5 Column structure enhancements
- [x] T12.2.5.1 Add PageTitle property to all recipe pages (no fallback to H1/H2) ✅ COMPLETED
- [x] T12.2.5.2 Add PageSummary property to all recipe pages (dedicated summaries) ✅ COMPLETED
- [x] T12.2.5.3 Update RecipeScanner to extract only PageTitle and PageSummary properties
  (show "unknown" if missing) ✅ COMPLETED
- [x] T12.2.5.4 Reorder columns: Chapter | Recipe | Title | Action | Summary | Location | Filename ✅ COMPLETED
- [x] T12.2.5.5 Implement responsive column priority (mobile/tablet/desktop) [SUPERSEDED BY T15.3]
- [x] T12.2.6 Add repository links to each recipe in overview page (post-deployment improvement)
- [ ] T12.3 Apply optical improvements for better user experience
- [ ] T12.3.3 Optimize table spacing and visual hierarchy
- [ ] T12.1 Fix existing bugs in overview functionality [LOW PRIORITY - AFTER MAIN IMPROVEMENTS]
- [ ] T12.1.1 Console logging cleanup (remove debug statements)
- [ ] T12.1.2 Error handling enhancement (show user-visible error messages)

- [ ] T8 Optimize render mode pages by reducing code duplication [AFTER OVERVIEW UPDATES]
- [ ] T8.2 Phase 2: Extract status card into shared component (base class already exists)
- [ ] T8.2.1 Create RenderModeStatusCard component with parameters
- [ ] T8.2.2 Integrate status card with WebAssembly page
- [ ] T8.2.3 Integrate status card with Server page
- [ ] T8.2.4 Integrate status card with Auto page
- [ ] T8.3 Phase 3: Additional optimizations (optional)
- [ ] T8.3.1 Extract Component Lifecycle Insight component (if beneficial)
- [ ] T8.3.2 Extract Educational Delay Indicator component (if beneficial)

- [ ] T7.5 Document educational comparison between all three modes [AFTER OVERVIEW UPDATES]
- [ ] T7.4 Show adaptive render mode switching (Server → Client) [BLOCKED: requires persistence]

## In Progress

- 🔄 T21.1.2 Implement ManifestGenerator class reusing existing RecipeScanner logic

## Done

- [x] T21.1.1 Create Tools/RecipeManifestGenerator project structure ✅ COMPLETED
- [x] T14.4.1 Document local production build testing methods in separate Markdown file ✅ COMPLETED
- [x] T14.3.8 Implement conditional HTTPS redirection for Azure App Service compatibility ✅ COMPLETED
- [x] T17 Update README.md to be more concise and helpful ✅ COMPLETED
- [x] T16 Implement version management system for BlazorCookbookApp ✅ COMPLETED
- [x] T14.2 Update README.md (deployment info, attribution, educational purpose) ✅ COMPLETED
- [x] T15.7 Desktop browser mobile emulation testing (Chrome DevTools, Firefox Responsive Design Mode) ✅ COMPLETED
- [x] T15.3 Implement mobile-first column priority system for recipe tables
  (Essential: Title/Action/Stars, Secondary: Summary/Location, Tertiary: Chapter/Recipe/Filename) ✅ COMPLETED
- [x] T15.1 Fix main recipe table mobile responsiveness by adding table-responsive wrapper ✅ COMPLETED
- [x] T14.7 Home Page Consistency and Content Optimization ✅ COMPLETED
- [x] T14.7.1 Remove "From Cookbook to Code" duplication from Resources page ✅ COMPLETED
- [x] T14.7.2 Replace generic "About This Project" with personal project purpose ✅ COMPLETED
- [x] T14.7.3 Add prominent navigation links to Learning Journey and Resources ✅ COMPLETED
- [x] T14.7.4 Remove "Coming Soon" and replace with continuous expansion messaging ✅ COMPLETED
- [x] T14.7.5 Add Featured Highlights section with 2 top recipes as teaser ✅ COMPLETED
- [x] T14.7.6 Home page content refinements (remove comprehensive, fix links, add book link, remove low-value sections) ✅ COMPLETED
- [x] T14.7.7 Fix book links (updated Packt URLs) and Auto page route (/ch01r04a) ✅ COMPLETED
- [x] T14.1.9 Remove About link from MainLayout.razor ✅ COMPLETED
- [x] T14.1 Update Resources Page and Create Learning Journey Page ✅ COMPLETED
- [x] T14.1.1 Update book descriptions to reflect actual usage (primary source, starting point, etc.) ✅ COMPLETED
- [x] T14.1.2 Create separate LearningJourney.razor page with enhanced content ✅ COMPLETED
- [x] T14.1.3 Add Learning Journey link to navigation with journal icon ✅ COMPLETED
- [x] T14.1.4 Update Resources page to link to Learning Journey instead of containing it ✅ COMPLETED
- [x] T14.1.5 Translate "überschaubares" to "manageable" in all content ✅ COMPLETED
- [x] T10.2.4 Step 4: Update Documentation (concise) ✅ COMPLETED
- [x] T10.2 Phase 3: Recipe Overview Enhancements ✅ COMPLETED
- [x] T13.8 Update RecipeScannerTests for property-based extraction ✅ COMPLETED
- [x] T13.7 Test featured section functionality ✅ COMPLETED
- [x] T13.6 Update RecipeInfo model to include Stars field ✅ COMPLETED
- [x] T13.5 Implement star display formatting (★★★★☆) ✅ COMPLETED
- [x] T13.4 Add Stars column to main recipe table ✅ COMPLETED
- [x] T13.3 Create featured recipes section component ✅ COMPLETED
- [x] T13.2 Update RecipeScanner to extract PageStars property ✅ COMPLETED
- [x] T13.1.3 Add PageStars property to remaining recipes (3 stars default) ✅ COMPLETED
- [x] T13.1.2 Add PageStars property to WebAssembly demo (4 stars) ✅ COMPLETED
- [x] T13.1.1 Add PageStars property to Recipe4 variants (5 stars each) ✅ COMPLETED
- [x] T13.1 Add PageStars property to all recipe pages ✅ COMPLETED
- [x] T13 Implement star rating system for recipe prioritization ✅ COMPLETED
- [x] T10.2.2 Step 2: Test Recipe Overview Integration (automatic) ✅ COMPLETED
- [x] T10.2.1.4 Update WebAssembly Demo page title to "WebAssembly Demo: Features Showcase" ✅ COMPLETED
- [x] T10.2.1.3 Update Auto page title to "Auto: Adaptive Server-to-Client" ✅ COMPLETED
- [x] T10.2.1.2 Update Server page title to "Server: Server-side Processing" ✅ COMPLETED
- [x] T10.2.1.1 Update WebAssembly page title to "WebAssembly: Client-side Processing" ✅ COMPLETED
- [x] T10.2.1 Step 1: Improve Recipe4 Titles and Summaries ✅ COMPLETED
- [x] T8.1 Phase 1: Extract common logic into base class ✅ COMPLETED
- [x] T8.1.1 Create RenderModeComponentBase with common fields and methods ✅ COMPLETED
- [x] T8.1.2 Convert WebAssembly page to inherit from base class ✅ COMPLETED
- [x] T8.1.3 Convert Server page to inherit from base class (cross-project) ✅ COMPLETED
- [x] T8.1.4 Convert Auto page to inherit from base class ✅ COMPLETED
- [x] T11.4.4 Test responsive layout of both pages ✅ COMPLETED
- [x] T11.4.3 Verify recipe discovery still works correctly ✅ COMPLETED
- [x] T11.4.2 Manual testing of navigation flow ✅ COMPLETED
- [x] T11.3.3 Test routing between Home and Browse Recipes ✅ COMPLETED
- [x] T11.3.2 Update any direct navigation links ✅ COMPLETED
- [x] T11.4.1 Run full test suite to verify no regressions ✅ COMPLETED
- [x] T11.5 UI Improvements ✅ COMPLETED
- [x] T11.5.1 Remove "Coming Soon" alert from Recipes page ✅ COMPLETED
- [x] T11.5.2 Remove direct links from recipe table (keep only Open buttons) ✅ COMPLETED
- [x] T11.5.3 Clean up placeholder content from Recipes page ✅ COMPLETED
- [x] T11.4.1 Run full test suite to verify no regressions ✅ COMPLETED
- [x] T11.3.1 Add "Browse Recipes" link to NavMenu.razor ✅ COMPLETED
- [x] T11.2 Restructure Home page for project introduction ✅ COMPLETED
- [x] T11.2.1 Replace recipe table with project overview content ✅ COMPLETED
- [x] T11.2.2 Add getting started section ✅ COMPLETED
- [x] T11.2.3 Add featured recipes or highlights ✅ COMPLETED
- [x] T11.2.4 Include link to Browse Recipes page ✅ COMPLETED
- [x] T11.1 Create new Browse Recipes page at `/recipes` route ✅ COMPLETED
- [x] T11.1.1 Create BlazorCookbookApp/Components/Pages/Recipes.razor ✅ COMPLETED
- [x] T11.1.2 Move recipe overview table from Home.razor to Recipes.razor ✅ COMPLETED
- [x] T11.1.3 Update page title to "Browse Recipes" ✅ COMPLETED
- [x] T11.1.4 Ensure RecipeScanner integration works correctly ✅ COMPLETED
- [x] T8.0.7 Create comprehensive unit tests for core business logic (96 tests) ✅ COMPLETED
- [x] T8.0.7.1 RecipeScannerTests.cs - 15 tests covering route patterns, summary extraction, variants ✅ COMPLETED
- [x] T8.0.7.2 RecipeInfoTests.cs - 11 tests covering data model properties and validation ✅ COMPLETED  
- [x] T8.0.7.3 RecipeUrlServiceTests.cs - 12 tests covering URL parsing and title formatting ✅ COMPLETED
- [x] T8.0.7.4 Fix Moq NavigationManager issues with custom TestNavigationManager ✅ COMPLETED
- [x] T8.0.7.5 Resolve line ending differences (\r\n vs \n) on Windows ✅ COMPLETED
- [x] T8.0.7.6 Verify all 96 tests pass - core business logic protected ✅ COMPLETED
- [x] T10.1 Ensure WebAssembly demo page appears in Recipe Overview ✅ COMPLETED
- [x] T10 Update Recipe Overview Page (Phase 1: Testing) ✅ COMPLETED
- [x] T9.5 Update comments in all render mode files ✅ COMPLETED
- [x] T9 Create WebAssembly Features Demo Page ✅ COMPLETED
- [x] T9.1 Create WebAssembly Demo Page ✅ COMPLETED
- [x] T9.1.1 Create WebAssemblyDemo.razor with interactive features ✅ COMPLETED
- [x] T9.1.2 Migrate demo content from main WebAssembly page ✅ COMPLETED
- [x] T9.1.3 Implement simplified action tracking for demo interactions ✅ COMPLETED
- [x] T9.2 Update Main WebAssembly Page ✅ COMPLETED
- [x] T9.2.1 Remove demo section from main WebAssembly page ✅ COMPLETED
- [x] T9.2.2 Add prominent link to demo page ✅ COMPLETED
- [x] T9.2.3 Condense performance benefits showcase ✅ COMPLETED
- [x] T9.3 Update Recipe Overview Integration ✅ COMPLETED
- [x] T9.3.1 Verify demo page appears in Recipe Overview ✅ COMPLETED
- [x] T9.3.2 Update page titles for clarity ✅ COMPLETED
- [x] T9.4 Documentation Updates ✅ COMPLETED
- [x] T9.4.1 Update optimization plan with demo separation ✅ COMPLETED
- [x] T9.4.2 Update README with demo page features ✅ COMPLETED
- [x] T8.0.6 Document test commands in development tips and test README ✅ COMPLETED
- [x] T8.0.5 Create manual test checklist for UI verification ✅ COMPLETED  
- [x] T8.0.4 Create test project README with TDD strategy documentation ✅ COMPLETED
- [x] T8.0.3 Write comprehensive unit tests for RenderModeComponentBase (TDD approach) ✅ COMPLETED
- [x] T8.0.2 Configure test project with bUnit, project references, and centralized packages ✅ COMPLETED
- [x] T8.0.1 Create BlazorCookbookApp.Tests project and add to solution ✅ COMPLETED
- [x] T8.0 Set up test infrastructure for Recipe4 optimization ✅ COMPLETED
- [x] T7.9.6 Update Auto page title to "Render mode InteractiveAuto" ✅ COMPLETED
- [x] T7.9.5 Fix Auto page status card to respect educational delay ✅ COMPLETED
- [x] T7.9.4 Update Server page title to "Render mode InteractiveServer" ✅ COMPLETED
- [x] T7.9.3 Fix Server page status card to respect educational delay ✅ COMPLETED
- [x] T7.9.2 Update WebAssembly page title to "Render mode InteractiveWebAssembly" ✅ COMPLETED
- [x] T7.9.1 Fix WebAssembly page status card to respect educational delay ✅ COMPLETED
- [x] T7.9 Fix status cards to show correct render mode during educational delay ✅ COMPLETED
- [x] T7.8.3 Update WebAssembly client page with consistent educational delay ✅ COMPLETED
- [x] T7.8.2 Add educational delay to Auto page with visual indicators ✅ COMPLETED
- [x] T7.8.1 Add educational delay to Server page with visual indicators ✅ COMPLETED
- [x] T7.8 Implement educational delay for static phase visibility ✅ COMPLETED
- [x] T7.7.4 Update Auto page to use singular "Previous state" terminology ✅ COMPLETED
- [x] T7.7.3 Add component lifecycle insight to Server page ✅ COMPLETED
- [x] T7.7.2 Update Server page to use singular "Previous state" and hide when no previous state ✅ COMPLETED
- [x] T7.7.1 Update Server page title to "Server Render Mode Status" ✅ COMPLETED
- [x] T7.7 Apply consistent UI improvements to Server page ✅ COMPLETED
- [x] T7.6.3 Add explanation about component instance recreation behavior ✅ COMPLETED
- [x] T7.6.2 Only show previous modes section when there are actual previous states ✅ COMPLETED  
- [x] T7.6.1 Rename "Render Mode Journey" to "Previous render modes" ✅ COMPLETED
- [x] T7.6 Update OfferAuto UI based on component lifecycle discoveries ✅ COMPLETED
- [x] T8.3 Remove duplicate RenderAction.cs files ✅ COMPLETED
- [x] T8.2 Document shared types placement strategy ✅ COMPLETED
- [x] T8.1 Add Microsoft's recommended project reference pattern (Server → Client) ✅ COMPLETED
- [x] T8 Document project reference structure insights in development tips ✅ COMPLETED
- [x] T7.1 Create OfferAuto.razor with InteractiveAuto render mode ✅ COMPLETED
- [x] T7.0 Discovered InteractiveAuto component lifecycle behavior (instance recreation)
- [x] T6.10 Add "Time to Interactive" display showing duration until interactivity achieved
- [x] T6.9 Remove artificial delay from server version for authentic timing
- [x] T6.8 Add duration tracking to action history (milliseconds since start)
- [x] T6.6 Add action timeline display with real timing
- [x] T6.5 Create RenderAction data model for action history
- [x] T6.4 Implement authentic render mode journey tracking (no artificial delays)
- [x] T6.1 Create server version with InteractiveServer render mode (/ch01r04s)
- [x] T5 Improve render mode display with color-coded Bootstrap styling
- [x] T5.1 Add artificial delay for demo visibility
- [x] T5.2 Create professional card layout for render mode status
- [x] T5.3 Implement color coding (Green=WebAssembly, Yellow=Static)
- [x] T5.4 Fix ugly light blue color scheme
- [x] T4 Clean up Recipe4 page by removing debug output section
- [x] T3 Create RecipeUrlService for URL parameter extraction across all recipe pages
- [x] T3.1 Implement service with chapter/recipe extraction from URLs
- [x] T3.2 Add formatted title generation functionality
- [x] T3.3 Register service in DI container
- [x] T3.4 Update existing recipe pages to use the service
- [x] T3.5 Fix DI registration for InteractiveWebAssembly render mode (server + client)
- [x] T3.6 Add comprehensive XML documentation to service
- [x] T1 Optimize page Rendermodes ch1r4
- [x] T1.1 Make better visible what the page is about
- [x] T1.2 Separate render mode detection from the page title
- [x] T14.3.7 Create comprehensive Azure deployment checklist ✅ COMPLETED
- [x] T14.3.6 Document recommended Azure App Service settings for Linux deployment ✅ COMPLETED
- [x] T14.3.4 Test publish output to ensure WebAssembly files are included ✅ COMPLETED
- [x] T14.3.3 Verify Program.cs WebAssembly and static file configuration ✅ COMPLETED
- [x] T14.3.2 Create appsettings.Production.json with optimized logging levels (Linux deployment) ✅ COMPLETED
- [x] T14.3.5 Verify appsettings files are included in version control ✅ COMPLETED

## Deferred

- [ ] T15.2 Set up automated mobile testing with Playwright (device emulation, real browser testing, responsive design validation) [DEFERRED: Too complex for pre-deployment, focus on manual testing]
- [ ] T6 Create Recipe4 Server vs Client comparison implementation [DEFERRED: Not current priority]
- [ ] T6.2 Update client version with authentic state tracking and distinct page title [DEFERRED]
- [ ] T6.3 Create comparison page with responsive layout (/ch01r04c) [DEFERRED]
- [ ] T6.7 Test Recipe Overview integration for all three versions [DEFERRED]
- [ ] T6.8 Test mobile responsiveness and stacked layout [DEFERRED]
- [ ] T6.9 Validate educational value with authentic behavior [DEFERRED]

## Notes

- Use `- [ ]` for incomplete tasks
- Use `- [x]` for completed tasks
- Move tasks between sections as needed
- Use B + number for bug tasks
- **Task Order**: In Done section, place newer tasks at the top, older tasks at the bottom
- **Authentic Behavior**: Focus on real render mode behavior, not artificial delays
- **Mobile Testing Strategy**: Desktop browser emulation for pre-deployment, actual device testing post-deployment

## Examples

- [ ] T1 Short description of simple task
- [ ] T2 Short description of main task
- [ ] T2.1 Short description of subtask
