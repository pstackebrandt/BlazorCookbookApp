# Manual Test Checklist for Recipe4 Optimization

This checklist should be used to manually verify functionality before and after
implementing the base class optimization.

## Pre-Implementation Baseline Testing

Test all three render mode pages to establish baseline behavior:

### 1. WebAssembly Page (`/ch01r04`)

- [ ] Page loads successfully
- [ ] Shows "🕐 Educational delay: Showing static rendering phase for 1500ms..."
- [ ] Status card shows "Static" (yellow) initially
- [ ] After 1.5 seconds, status card changes to "WebAssembly" (green)
- [ ] Interactive status changes from False to True
- [ ] Component lifecycle insight displays correctly
- [ ] Action history shows button clicks after interactivity
- [ ] Page title shows "Render mode InteractiveWebAssembly"

### 2. Server Page (`/ch01r04s`)

- [ ] Page loads successfully
- [ ] Shows "🕐 Educational delay: Showing static rendering phase for 1500ms..."
- [ ] Status card shows "Static" (yellow) initially
- [ ] After 1.5 seconds, status card changes to "Server" (blue)
- [ ] Interactive status changes from False to True
- [ ] Component lifecycle insight displays correctly
- [ ] Action history shows button clicks after interactivity
- [ ] Previous state section only shows when there are previous states
- [ ] Page title shows "Render mode InteractiveServer"

### 3. Auto Page (`/ch01r04a`)

- [ ] Page loads successfully
- [ ] Shows "🕐 Educational delay: Showing static rendering phase for 1500ms..."
- [ ] Status card shows "Static" (yellow) initially
- [ ] After 1.5 seconds, status card changes to "WebAssembly" (green) [[memory:3760376716433116511]]
- [ ] Interactive status changes from False to True
- [ ] Component lifecycle insight displays correctly
- [ ] Action history shows button clicks after interactivity
- [ ] Previous render modes section only shows when there are previous modes
- [ ] Page title shows "Render mode InteractiveAuto"

## Post-Implementation Verification Testing

After implementing the base class, repeat all tests above to ensure:

### Functional Equivalence

- [ ] All behavior from baseline testing still works identically
- [ ] No visual differences in UI
- [ ] Same timing behavior (1.5 second delay)
- [ ] Same color coding (Static=yellow, Server=blue, WebAssembly=green)

### Performance Verification

- [ ] Page load times are similar or better
- [ ] No console errors or warnings
- [ ] Memory usage appears normal

### Code Quality Verification

- [ ] No duplicate code remains in the three pages
- [ ] Base class is properly referenced
- [ ] Page-specific functionality still works (Server action history,
  Auto journey tracking)

## Browser Testing Matrix

Test in multiple browsers to ensure compatibility:

- [ ] Chrome/Edge (Chromium)
- [ ] Firefox
- [ ] Safari (if available)

## Error Scenarios

Test error handling:

- [ ] Clear browser cache and test fresh loads
- [ ] Test with slow network connection
- [ ] Test with JavaScript disabled (should show static only)

## Notes Section

Use this space to record any issues found during testing:

```text
Date: ___________
Tester: ___________

Issues Found:
-
-
-

Resolved:
-
-
-
```

## Automated Test Status

- [ ] All unit tests pass (`dotnet test`)
- [ ] No build warnings or errors
- [ ] Code coverage meets requirements (>80% for base class)
