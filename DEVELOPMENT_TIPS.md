# Development Tips

## Blazor Project Structure

### Project Reference Structure (Microsoft Recommended)

**Server project should reference Client project** - This is Microsoft's official recommendation for Blazor Web Apps:

- **Deployment**: Server hosts and serves the client application
- **Bundling**: Server includes the client's WebAssembly bundle in its output  
- **Shared Types**: Server can access types defined in client project for API contracts
- **Architecture**: Follows typical web application pattern where server is the entry point

### Avoiding Duplicate Files

- **Shared Types**: Place shared types (like `RenderAction.cs`) in the client project only
- **Server Access**: Server project can access client types through project reference
- **Maintenance**: Eliminates duplicate code and maintenance burden
- **Rule**: Since `InteractiveAuto` components must be in client project, keep shared types there too

## Blazor Render Modes

### Component Project Placement Rules

| Render Mode              | Required Location  | Reason                                        |
| ------------------------ | ------------------ | --------------------------------------------- |
| `Static`                 | Server Project     | No client execution needed                    |
| `InteractiveServer`      | Server Project     | Executes only on server                       |
| `InteractiveWebAssembly` | **Client Project** | Must be in WASM bundle                        |
| `InteractiveAuto`        | **Client Project** | Must support both server and client execution |

**Key Rule**: `InteractiveAuto` and `InteractiveWebAssembly` components MUST be in the client project to be included in the WebAssembly bundle for client-side execution.

### InteractiveWebAssembly & InteractiveAuto Components

- **Service Registration**: Require service registration in both server and client projects
  - Components pre-render on the server first, then hydrate on the client
  - Server needs the service for pre-rendering phase
  - Client needs the service for interactive phase
  - Register the same service in both `BlazorCookbookApp/Program.cs` and `BlazorCookbookApp.Client/Program.cs`

## Testing

### Recipe4 Optimization Testing Commands

```bash
# When ready to run tests (after stopping Blazor app):
dotnet test BlazorCookbookApp.Tests

# To run tests continuously during development:
dotnet test BlazorCookbookApp.Tests --watch

# To see detailed test output:
dotnet test BlazorCookbookApp.Tests --verbosity normal
```

**Important**: Stop the running Blazor application before testing to avoid file lock issues.

## Best Practices

### Service Implementation

- Use interfaces for dependency injection to enable testing and flexibility
- Add comprehensive XML documentation for IntelliSense support
- Handle error cases gracefully (e.g., URL parsing failures)

### Project Structure

- Keep shared services in appropriate namespaces
- Add global using statements in `_Imports.razor` for commonly used namespaces
- Use scoped services for request-specific data that doesn't need to persist

## Testing Strategy

### Phase 1: Core Business Logic Protection ✅ COMPLETED
- Created comprehensive test coverage (96 tests) for core components
- Protected RecipeScanner, RecipeInfo, and RecipeUrlService during restructuring
- All tests passing with 0 failures

### Phase 2: Home Page Restructuring (IN PROGRESS)
- ✅ Created Browse Recipes page at `/recipes` with placeholder content
- ✅ Added navigation menu link for "Browse Recipes"
- 🔄 Next: Move recipe overview table from Home.razor to Recipes.razor

## Test Commands

```bash
# Run all tests
dotnet test

# Run specific test categories
dotnet test --filter "RecipeScanner*"
dotnet test --filter "*RecipeInfo*" 
dotnet test --filter "*RecipeUrlService*"

# Run with detailed output
dotnet test --verbosity detailed
```

## Navigation Structure

### Current (Phase 2.1)
- **Home (`/`)**: Recipe overview table (current functionality)
- **Browse Recipes (`/recipes`)**: Placeholder content with categories and getting started guide
- **Navigation**: Menu includes both pages

### Future (Post-Restructuring)
- **Home (`/`)**: Project introduction and getting started content
- **Browse Recipes (`/recipes`)**: Complete recipe overview table with auto-discovery
- **Navigation**: Menu provides clear separation of concerns

## Files Created/Modified

### New Files
- `BlazorCookbookApp/Components/Pages/Recipes.razor` - Browse Recipes page

### Modified Files  
- `BlazorCookbookApp/Components/Layout/NavMenu.razor` - Added Browse Recipes link
- `TASKS.md` - Updated task status
- `docs/Recipe-Overview-Plan.md` - Updated planning documentation

## Next Steps

1. **Move recipe overview table** from Home.razor to Recipes.razor
2. **Restructure Home page** for project introduction content
3. **Test navigation flow** between pages
4. **Verify responsive layout** on both pages
