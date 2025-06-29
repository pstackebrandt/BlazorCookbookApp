# BlazorCookbook Style Guide

## Color Scheme Standards

### Bootstrap Badge Colors

- 🟢 **Green** (`bg-success`): Active states, positive status (Interactive: true, successful operations)
- 🟡 **Yellow** (`bg-warning`): Static render mode, transitional states, warnings
- 🔵 **Blue** (`bg-primary`): Server render mode indicators (server-specific features)
- ⚫ **Gray** (`bg-secondary`): All timing information, neutral states, measurements
- 🔴 **Red** (`bg-danger`): Error states, failures (when needed)

### Card Styling

- **Clean backgrounds**: Use default Bootstrap card styling without custom borders
- **Dark headers**: `bg-dark text-white` for card headers with important status information
- **Consistent spacing**: `mb-4` for card margins, standard Bootstrap padding

### Timing and Measurement Display

- **All timing badges**: Use `bg-secondary text-white` for consistency
- **Format**: Display milliseconds as `"123ms"` with `ToString("F0")`
- **Grouping**: Group related timing information together

### Render Mode Status Display

```html
<!-- Standard render mode status pattern -->
<div class="card mb-4">
    <div class="card-header bg-dark text-white">
        <h5 class="mb-0">🔍 Current Render Mode Status</h5>
    </div>
    <div class="card-body">
        <!-- Status content -->
    </div>
</div>
```

## Component Architecture Patterns

### Recipe Metadata Constants

```razor
@code {
    // Recipe metadata constants (used by RecipeScanner)
    private const string RECIPE_TITLE = "Recipe name";
    private const string RECIPE_SUMMARY = "Brief description with technical details and user benefits";
}
```

### Page Structure Template

```razor
@page "/ch##r##[variant]"
@rendermode [RenderMode]

<PageTitle>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</PageTitle>
<h1>@RecipeUrlService.GetTitleWithNumbers(RECIPE_TITLE)</h1>
<p class="recipe-summary lead">@RECIPE_SUMMARY</p>

<!-- Render mode detection section -->
<!-- Status card with consistent styling -->
<!-- Main content -->
<!-- Features list -->
```

### Component Lifecycle Insight Pattern

```razor
<!-- Component Lifecycle Explanation -->
<div class="mt-3 p-3 bg-light rounded">
    <h6 class="text-muted mb-2">💡 Component Lifecycle Insight</h6>
    <p class="small mb-1">
        <strong>Important:</strong> Each render mode transition creates a fresh component instance. 
        In-memory state (like action history) doesn't survive transitions without persistence mechanisms.
    </p>
    <p class="small mb-0 text-muted">
        This behavior is normal Blazor architecture - what you see as "one page" is actually multiple component instances across render phases.
    </p>
</div>
```

### Action History Pattern

```razor
<div class="mt-3">
    <h6>Current Session Actions:</h6>
    @foreach(var category in GetActionsByCategory())
    {
        <div class="mb-2">
            <strong>@category.Key:</strong>
            <ul class="list-unstyled ms-3">
                @foreach(var action in category.Value)
                {
                    <li>
                        <span class="badge bg-light text-dark">@action.Time</span>
                        <span class="badge bg-secondary text-white">@($"{action.DurationMs.ToString("F0")}ms")</span>
                        @action.Description
                    </li>
                }
            </ul>
        </div>
    }
</div>
```

## Content Guidelines

### Recipe Titles

- **Length**: 3-8 words maximum
- **Format**: Descriptive noun phrase
- **Examples**: "Render modes", "Component parameters", "Server components"

### Recipe Summaries

- **Length**: 10-25 words (1-2 sentences)
- **Content**: Include both technical aspects and user benefits
- **Format**: "Technical approach with user benefit" or naturally combined
- **Examples**:
  - "Client-side rendering with WebAssembly download and local processing"
  - "Server-side rendering with SignalR communication for real-time updates"

### Action Descriptions

- **Categories**: Initialization, Transition, Active, Interaction, ServerPhase, ClientTransition, ClientActive
- **Format**: Clear, present-tense descriptions
- **Examples**: "Component initialization started", "Transitioned to Server mode"
- **Timing**: Always include duration badges with consistent formatting

### Previous State Display

- **Terminology**: Use singular "Previous state:" not "Previous render modes:"
- **Conditional Display**: Only show when previous state differs from current state
- **Logic**: `@if (!string.IsNullOrEmpty(_initialRenderMode) && _initialRenderMode != RendererInfo.Name)`

### Educational Delay Pattern

- **Purpose**: Make static rendering phase visible for educational value
- **Duration**: `1500ms` (1.5 seconds) consistent across all render mode pages
- **Implementation**: `private const int STATIC_PHASE_DELAY_MS = 1500;`
- **Visual Indicator**: `🕐 Educational delay: Showing static rendering phase for {STATIC_PHASE_DELAY_MS}ms...`
- **Conditional Logic**: `@if (!RendererInfo.IsInteractive || _isDelayed)`

#### **CRITICAL PRINCIPLE: Educational Delays Must Be Real**

**Rule**: Educational delays MUST be actual execution delays using `await Task.Delay()`, not visual-only simulations.

**Implementation Requirements:**
- ✅ **Real State Delay**: Use `await Task.Delay(STATIC_PHASE_DELAY_MS)` in component lifecycle
- ✅ **Delayed State Changes**: Component state updates occur AFTER delay completes
- ✅ **Delayed UI Updates**: Call `StateHasChanged()` AFTER delay, not before
- ✅ **Authentic Timing**: Delay affects actual component lifecycle measurements

**Code Pattern:**
```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender && _isDelayed)
    {
        await Task.Delay(STATIC_PHASE_DELAY_MS);  // REAL delay
        _isDelayed = false;
        StateHasChanged();  // UI updates AFTER delay
    }
}
```

**Rationale:**
- **Educational Value**: Users experience authentic Blazor component lifecycle timing
- **Realistic Simulation**: Mimics actual static rendering phase duration
- **Accurate Measurements**: Provides genuine performance data when delay is subtracted
- **Observable Transitions**: Makes fast render mode changes visible for learning

### Status Card Display Methods

- **Purpose**: Ensure status cards respect educational delay and show realistic progression
- **Display Methods**: 
  ```csharp
  private string GetDisplayRenderMode() => _isDelayed ? "Static" : (RendererInfo.Name ?? "Unknown");
  private bool GetDisplayInteractive() => !_isDelayed && RendererInfo.IsInteractive;
  ```
- **Usage**: Use `@GetDisplayRenderMode()` and `@GetDisplayInteractive()` instead of direct `RendererInfo` properties
- **Color Logic**: Base `GetRenderModeClass()` on `GetDisplayRenderMode()` result
- **Future**: These methods will be extracted to `RenderModeComponentBase` (see Recipe4-Optimization-Plan.md)

## Responsive Design

### Table Layouts

- Use `table-responsive` class for horizontal scroll on small screens
- Truncate summary text: ~50 characters mobile, ~80 characters tablet
- Maintain table structure across all devices

### Mobile Considerations

- Stack content vertically when appropriate
- Ensure touch-friendly button sizes
- Test on multiple screen sizes

## CSS and Styling Standards

### CSS Classes

- **Recipe summaries**: `recipe-summary lead`
- **Status cards**: Standard Bootstrap card classes
- **Timing badges**: `badge bg-secondary text-white`
- **Status badges**: Use semantic colors (success, warning, primary)

## UI Consistency Guidelines

### Timing Display Standards

- **Format**: Display milliseconds as `"123ms"` with `ToString("F0")`
- **Color**: Always use `bg-secondary text-white` for timing badges
- **Grouping**: Group related timing information in the same section

## Accessibility

### Semantic HTML

- Use proper heading hierarchy (H1, H2, H3)
- Include meaningful alt text for icons/images
- Ensure proper contrast ratios

### Bootstrap Integration

- Leverage Bootstrap's built-in accessibility features
- Use semantic color classes appropriately
- Ensure keyboard navigation works properly
