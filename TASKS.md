# Tasks

## Todo

- [ ] T10 Update Recipe Overview Page
- [ ] T10.1 Ensure WebAssembly demo page appears in Recipe Overview
- [ ] T10.2 Update Recipe Overview to show clear distinction between render mode pages and demo
- [ ] T10.3 Verify Recipe Overview integration with all Recipe4 variants
- [ ] T10.4 Test Recipe Overview responsive layout and navigation
- [ ] T8 Optimize render mode pages by reducing code duplication
- [ ] T8.1 Phase 1: Extract common logic into base class
- [ ] T8.1.1 Create RenderModeComponentBase with common fields and methods
- [ ] T8.1.2 Convert WebAssembly page to inherit from base class
- [ ] T8.1.3 Convert Server page to inherit from base class (cross-project)
- [ ] T8.1.4 Convert Auto page to inherit from base class
- [ ] T8.2 Phase 2: Extract status card into shared component (if Phase 1 successful)
- [ ] T8.2.1 Create RenderModeStatusCard component with parameters
- [ ] T8.2.2 Integrate status card with WebAssembly page
- [ ] T8.2.3 Integrate status card with Server page
- [ ] T8.2.4 Integrate status card with Auto page
- [ ] T8.3 Phase 3: Additional optimizations (optional)
- [ ] T8.3.1 Extract Component Lifecycle Insight component (if beneficial)
- [ ] T8.3.2 Extract Educational Delay Indicator component (if beneficial)
- [ ] T7.5 Document educational comparison between all three modes
- [ ] T7.4 Show adaptive render mode switching (Server → Client) [BLOCKED: requires persistence]
- [ ] T7.3 Add WebAssembly download progress indication [BLOCKED: requires persistence]
- [ ] T7.2 Implement first-visit vs subsequent-visit behavior tracking [BLOCKED: requires persistence]

## In Progress

## Done

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

## Deferred

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

## Examples

- [ ] T1 Short description of simple task
- [ ] T2 Short description of main task
- [ ] T2.1 Short description of subtask
