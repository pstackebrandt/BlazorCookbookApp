# Tasks

---

## Next Actions

- **P1**: T21 Implement Build-time Recipe Manifest (Phase 1)
- **P2**: T14.4 Application Performance Optimization
- **P3**: T12 Overview Page Structural and Optical Improvements

---

## Todo

- [ ] **P1** T21 Implement Build-time Recipe Manifest (Phase 1)
  - [ ] T21.1 Create Recipe Manifest Generator console application
  - [ ] T21.1.3 Create RecipeManifest model classes
  - [ ] T21.1.4 Add JSON serialization and file output
  - [ ] T21.1.5 Add PageVisibleInOverview property support and recipe exclusion logic
  - [ ] T21.2 Update RecipeScanner service for JSON loading
  - [ ] T21.2.1 Create IManifestLoader interface and implementation
  - [ ] T21.2.2 Add configuration options in appsettings.json
  - [ ] T21.2.3 Implement JSON loading with file scanning fallback
  - [ ] T21.2.4 Add logging for manifest loading operations
  - [ ] T21.2.5 Implement recipe visibility filtering in JSON loading
  - [ ] T21.3 Integration and testing
  - [ ] T21.3.1 Add manifest generation to Production Build Guide
  - [ ] T21.3.2 Test manifest generation and JSON loading
  - [ ] T21.3.3 Create unit tests for manifest functionality
  - [ ] T21.3.4 Test production build with JSON manifest
  - [ ] T21.3.6 Implement automatic multi-mode testing
  - [ ] T21.3.7 Test recipe visibility filtering and exclusion logic
  - [ ] T21.3.8 Test admin view functionality and hidden recipe access
  - [ ] T21.4 Deployment verification
  - [ ] T21.4.1 Deploy with manifest and verify Browse Recipes page works
  - [ ] T21.4.2 Test error scenarios and fallback behavior
  - [ ] T21.4.3 Verify hidden recipes are excluded from Browse Recipes in production
  - [ ] T21.4.4 Verify admin view works for hidden recipe management in production

- [ ] **P2** T14 Azure Deployment Preparation
  - [ ] T14.4 Application Performance Optimization (bundle size, static assets, caching)

- [ ] **P1** T18 Recipe Overview Page – Publish Strategies
  - [ ] T18.4 Implement build-time manifest (see T21 tasks for details)

- [ ] **P3** T15 Mobile Responsiveness and Testing
  - [ ] T15.6 Manual mobile testing on actual phone devices

- [ ] **P3** T12 Overview Page Structural and Optical Improvements
  - [ ] T12.2 Implement structural improvements for better data display
    - [ ] T12.2.1 Add summary truncation for long text
    - [ ] T12.2.3 Add basic sorting functionality (Chapter/Recipe/Location)
    - [ ] T12.2.4 Add search/filter functionality
    - [ ] T12.2.5 Column structure enhancements
  - [ ] T12.3 Apply optical improvements for better user experience
    - [ ] T12.3.3 Optimize table spacing and visual hierarchy
  - [ ] T12.1 Fix existing bugs in overview functionality [LOW PRIORITY]
    - [ ] T12.1.1 Console logging cleanup
    - [ ] T12.1.2 Error handling enhancement

- [ ] **P3** T27 Update /ch01r03cl page: Make it easily visible that only console output is expected.
- [ ] **P3** T28 Update /ch01r05 page: Make it easily visible that only console output is expected.

---

## In Progress

(No tasks currently in progress)

---

## Deferred

- [ ] ⏸️ T22 Responsive design issue (layout doesn't switch to mobile) ❌ FAILED
  - Attempted fix failed (broke menu), reverted.
- [ ] ⏸️ T24 Resources Page Visual Consistency Improvements
- [ ] ⏸️ T25 Home Page Developer Profile Emphasis
- [ ] ⏸️ T26 Fix PageVisibleInOverview Warnings
- [ ] ⏸️ T15.2 Set up automated mobile testing with Playwright [DEFERRED] Too complex.
- [ ] ⏸️ T6 Create Recipe4 Server vs Client comparison [DEFERRED] Not current priority.

---

## Backlog

- [ ] **P4** T23 Debug View Enhancements
  - [ ] T23.1 Fix base64 obfuscation implementation
  - [ ] T23.2 Add debug statistics panel with structural separation
  - [ ] T23.2.1 Create separate DebugInfoPanel component
  - [ ] T23.2.2 Add visible/hidden recipe counts
  - [ ] T23.2.3 Add manifest vs fallback usage indicator
  - [ ] T23.2.4 Add fallback activation status display
  - [ ] T23.2.5 Style debug panel with clear visual separation
  - [ ] T23.3 Remove temporary debug console logging
- [ ] **P4** T8 Optimize render mode pages by reducing code duplication
  - [ ] T8.2 Phase 2: Extract status card into shared component
    - [ ] T8.2.1 Create RenderModeStatusCard component
    - [ ] T8.2.2 Integrate status card with WebAssembly page
    - [ ] T8.2.3 Integrate status card with Server page
    - [ ] T8.2.4 Integrate status card with Auto page
  - [ ] T8.3 Phase 3: Additional optimizations (optional)
    - [ ] T8.3.1 Extract Component Lifecycle Insight component
    - [ ] T8.3.2 Extract Educational Delay Indicator component
- [ ] **P4** T7.5 Document educational comparison between all three modes
- [ ] **P4** T7.4 Show adaptive render mode switching (Server → Client) [BLOCKED]
- [ ] **P4** T29 Add screenshots of the app to README.md for visual appeal.

---

## Done

- [x] ✅ T30 Refactor and improve the TASKS.md file for better clarity and structure.
- [x] ✅ T21.3.1 Add manifest generation to Production Build Guide
- [x] ✅ T21.3.2 Test manifest generation and JSON loading
- [x] ✅ T21.3 Integration and testing
- [x] ✅ T21.2.5 Implement recipe visibility filtering in JSON loading (PageVisibleInOverview)
- [x] ✅ T21.2.4 Add logging for manifest loading operations
- [x] ✅ T21.2.3 Implement JSON loading with file scanning fallback
- [x] ✅ T21.2.2 Add configuration options in appsettings.json
- [x] ✅ T21.2.1 Create IManifestLoader interface and implementation
- [x] ✅ T21.2 Update RecipeScanner service for JSON loading
- [x] ✅ T21.1.5 Add PageVisibleInOverview property support and recipe exclusion logic
- [x] ✅ T21.1.4 Add JSON serialization and file output
- [x] ✅ T21.1.3 Create RecipeManifest model classes
- [x] ✅ T21.1.2 Implement ManifestGenerator class reusing existing RecipeScanner logic
- [x] ✅ T21.3.5a Fix test file route patterns and enhanced functionality
- [x] ✅ T21.3.5 Add test files with PageVisibleInOverview=false for testing
- [x] ✅ T21.2.7 Implement debug view security with base64 obfuscation
- [x] ✅ T21.2.6 Add debug view functionality for hidden recipe management
- [x] ✅ T21.1.1 Create Tools/RecipeManifestGenerator project structure
- [x] ✅ T20 Fix Recipe Overview Build Error (NETSDK1022)
- [x] ✅ T19 Consolidate production build docs
- [x] ✅ T18 Recipe Overview Page – Publish Strategies (Initial parts)
- [x] ✅ T17 Update README.md to be more concise and helpful
- [x] ✅ T16 Implement version management system for BlazorCookbookApp
- [x] ✅ T15 Documentation Reorganization Series
- [x] ✅ T15 Mobile Responsiveness and Testing (Initial parts)
- [x] ✅ T14 Azure Deployment Preparation (Most parts)
- [x] ✅ T14.4.1 Document local production build testing methods
- [x] ✅ T14.3.8 Implement conditional HTTPS redirection for Azure
- [x] ✅ T13 Implement star rating system for recipe prioritization
- [x] ✅ T12.2.2 Implement responsive table design for mobile
- [x] ✅ T12.2.5.1 Add PageTitle property to all recipe pages
- [x] ✅ T12.2.5.2 Add PageSummary property to all recipe pages
- [x] ✅ T12.2.5.3 Update RecipeScanner to extract only PageTitle and PageSummary
- [x] ✅ T12.2.5.4 Reorder columns in recipe overview
- [x] ✅ T12.2.5.5 Implement responsive column priority [SUPERSEDED BY T15.3]
- [x] ✅ T12.2.6 Add repository links to each recipe in overview page
- [x] ✅ T11 Restructure Home page and create Browse Recipes page
- [x] ✅ T10 Update Recipe Overview Page (Phase 1: Testing)
- [x] ✅ T9 Create WebAssembly Features Demo Page
- [x] ✅ T8.1 Phase 1: Extract common logic into base class
- [x] ✅ T8.0 Set up test infrastructure for Recipe4 optimization
- [x] ✅ T7 Initial Render Mode implementation and improvements (Auto, Server, WASM)
- [x] ✅ T1-T6 Initial recipe implementations and service creation

---

## Notes

- Use `- [ ]` for incomplete tasks.
- Use `- [x] ✅` for completed tasks.
- Use `- [x] ⏸️` for deferred tasks.
- **Task Order**: In the Done section, newer tasks are at the top.
- **Priorities**: P1 (Highest) to P4 (Lowest). P5 tasks are not planned.
- **Linking**: For tasks related to specific files, consider adding a markdown link to the file for quick navigation.

---

## Examples

- [ ] **P1** T1 Short description of a high-priority task
- [ ] **P3** T2 Short description of a medium-priority task
  - [ ] T2.1 Short description of subtask
- [x] ✅ T3 Example of a completed task
