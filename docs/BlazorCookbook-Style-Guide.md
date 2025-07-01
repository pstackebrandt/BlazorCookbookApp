# BlazorCookbook Style Guide

## Color Scheme Standards

### Bootstrap Badge Color Semantics

Our color system follows a clear visual hierarchy based on relevance and state significance:

- 🟢 **Green** (`bg-success`): **Current results** - Active states, positive
  status, successful operations, current phase indicators
  - Examples: Interactive: True, Current Phase: "Client-side processing",
    successful operations
  - Meaning: This is the current, expected, successful state

- 🟡 **Yellow** (`bg-warning`): **Previous/Temporary states** - Historical
  states, transitions, temporary but relevant information
  - Examples: Previous render modes (Static¹), transitional phases,
    temporary status indicators
  - Meaning: This was relevant in the past or is temporarily significant

- ⚫ **Gray** (`bg-secondary`): **Specific but less relevant** - Technical
  details, measurements, metadata that's noteworthy but not primary
  - Examples: Timing information, assigned render modes, duration badges,
    technical specifications
  - Meaning: More noteworthy than common text, but supporting information
    rather than primary status

- 🔵 **Blue** (`bg-primary`): **Reserved for future use** - Available for
  special categorization as needs arise
  - Currently unused - reserved for future semantic needs
  - Meaning: To be determined based on application requirements

- 🔴 **Red** (`bg-danger`): **Errors and failures** - Error states,
  failures, critical issues (when needed)
  - Examples: Error messages, failed operations, critical warnings
  - Meaning: Something went wrong or requires immediate attention

### Color Application Guidelines

**Decision Framework**: When choosing badge colors, ask:
1. **Is this the current, active state?** → 🟢 **Green**
2. **Is this previous/historical but relevant?** → 🟡 **Yellow**
3. **Is this technical detail/metadata?** → ⚫ **Gray**
4. **Is this an error/failure?** → 🔴 **Red**
5. **Special case requiring future categorization?** → 🔵 **Blue** (reserved)

**Practical Examples**:
- `<span class="badge bg-success">Server</span>` ← Current render mode
- `<span class="badge bg-warning">Static¹</span>` ← Previous state with footnote
- `<span class="badge bg-secondary">InteractiveServerRenderMode</span>` ← Technical assignment
- `<span class="badge bg-secondary">1234ms</span>` ← Timing measurement

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

#### **CRITICAL PRINCIPLE: Truthful State Display**

> **⚠️ UPDATED**: See `Truthful-State-Design-Principle.md` for the current approach.

**Rule**: Component state displays MUST show actual, truthful state at every moment, not artificial or simulated state.

**Implementation Requirements:**
- ✅ **Truthful State Display**: Always show actual render mode and interactive status
- ✅ **Educational Context**: Provide learning context through explanations, not state masking
- ✅ **Timing Transparency**: Separate real timing from educational delays
- ✅ **Authentic Journey**: Track observable component state transitions

**Code Pattern:**
```csharp
// CORRECT: Always show actual state
protected string GetActualRenderMode() => GetCurrentRenderMode() ?? "Unknown";
protected bool GetActualInteractive() => GetCurrentInteractive();

// Educational context through explanation, not state masking
<div class="alert alert-info">
    <strong>📚 Educational Context:</strong> This component started with server-side pre-rendering...
</div>
```

**Rationale:**
- **Accurate Learning**: Users learn actual Blazor component behavior
- **Debugging Skills**: Users see what they would see in real applications
- **Performance Awareness**: Real timing data separated from artificial delays
- **Transparent Education**: Clear distinction between real behavior and educational aids

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
