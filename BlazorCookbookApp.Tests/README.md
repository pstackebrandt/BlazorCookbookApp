# BlazorCookbookApp.Tests

This test project provides automated testing for the Recipe4 render mode
optimization process.

## Test Strategy

We're using a **Test-Driven Development (TDD)** approach for the base class extraction:

1. **Write tests first** - Define expected behavior through unit tests
2. **Implement to satisfy tests** - Create the base class to pass all tests
3. **Refactor with confidence** - Use tests to ensure no regressions during optimization

## Test Structure

### Unit Tests

- `Shared/RenderModeComponentBaseTests.cs` - Tests for the common base class functionality
- Tests cover display methods, CSS class generation, delay logic, and initialization

### Manual Testing

- `ManualTestChecklist.md` - Comprehensive checklist for manual verification
- Covers all three render mode pages before and after optimization
- Includes browser compatibility and error scenario testing

## Test Infrastructure

### Packages Used

- **xUnit** - Primary testing framework
- **bUnit** - Blazor component testing framework
- **Microsoft.NET.Test.Sdk** - .NET test SDK

### Project References

- References both `BlazorCookbookApp` and `BlazorCookbookApp.Client` projects
- Allows testing of shared components across both projects

## Running Tests

### Automated Tests

```bash
# When ready to run tests (after stopping Blazor app):
dotnet test BlazorCookbookApp.Tests

# To run tests continuously during development:
dotnet test BlazorCookbookApp.Tests --watch

# To see detailed test output:
dotnet test BlazorCookbookApp.Tests --verbosity normal

# Run specific test class
dotnet test --filter "RenderModeComponentBaseTests"
```

### Manual Tests

Follow the checklist in `ManualTestChecklist.md`:

1. Complete baseline testing before optimization
2. Implement base class changes
3. Verify all functionality still works identically

## Test Coverage Goals

- **Base Class**: >80% code coverage
- **Critical Paths**: 100% coverage for display methods and delay logic
- **Edge Cases**: All null/empty scenarios covered

## Development Workflow

1. **Before Changes**: Run baseline manual tests, document current behavior
2. **During Development**: Run unit tests frequently (`dotnet test --watch`)
3. **After Changes**: Complete full manual test checklist
4. **Before Commit**: Ensure all tests pass and no regressions

## Current Status

✅ **Test Infrastructure Setup Complete**

- Test project created and configured
- Unit tests written for base class (TDD approach)
- Manual test checklist prepared
- Ready to implement base class

⏳ **Next Steps**

1. Implement `RenderModeComponentBase` in `BlazorCookbookApp.Client/Shared/`
2. Run tests to verify implementation
3. Refactor WebAssembly page to use base class
4. Repeat for Server and Auto pages

## Notes

- Tests are written before implementation (TDD)
- Base class placeholder exists in test file for development
- Manual testing is crucial due to timing-dependent UI behavior
- File lock issues may occur if Blazor app is running during build
